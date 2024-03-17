using PixHub.Dtos;
using PixHub.Exceptions;
using PixHub.IdempotenceKeys;
using PixHub.Models;
using PixHub.Repositories;

namespace PixHub.Services;

public class PaymentsService(
  PaymentsRepository repository,
  MessageService messageService,
  PixKeyService pixKeyService,
  PaymentProviderAccountService accountService,
  PaymentProviderService paymentProviderService)
{
  readonly PaymentsRepository _repository = repository;
  readonly MessageService _messageService = messageService;
  readonly PixKeyService _pixKeyService = pixKeyService;
  readonly PaymentProviderAccountService _accountService = accountService;
  readonly PaymentProviderService _paymentProviderService = paymentProviderService;

  readonly int TOLERANCE_DUPLICATE_PAYMENTS_SECONDS = 30;

  public async Task<Payments?> CreatePayment(CreatePaymentDTO paymentDTO, string token)
  {
    PaymentProvider originProvider = await _paymentProviderService.FindByTokenAsync(token);

    AccountDTO originAccountDTO = paymentDTO.GetOriginAccount();
    PaymentProviderAccount originAccount = await _accountService.FindByUserCpf(
      paymentDTO.GetCpfUser(),
      originAccountDTO.Agency,
      originAccountDTO.Number);

    PixKey pixKey = await _pixKeyService
      .FindWithAccountAndProvider(paymentDTO.GetKeyType(), paymentDTO.GetKeyValue());

    PaymentProviderAccount destinyAccount = pixKey?.PaymentProviderAccount ??
      throw new PaymentProviderAccountNotFoundException("Destiny Payment Provider Account not found!");

    PaymentProvider destinyProvider = destinyAccount?.PaymentProvider ??
      throw new PaymentProviderNotFoundException();

    ValidateSelfTransaction(originAccount, destinyAccount);

    PaymentsIdempotenceKey key = new(paymentDTO.GetCpfUser(), paymentDTO.Amount, pixKey.Id);
    if (await IsDuplicatedPayment(key)) throw new DuplicatedPaymentException();

    Payments payment = await _repository.CreateAsync(paymentDTO.ToEntity(originAccount.Id, pixKey.Id));

    TransferPaymentDTO transferPaymentDTO =
      new(payment.Id, paymentDTO, originProvider.Webhook, destinyProvider.Webhook);

    _messageService.SendMessage(transferPaymentDTO);
    return payment;
  }

  private static void ValidateSelfTransaction(PaymentProviderAccount origin, PaymentProviderAccount destiny)
  {
    if (destiny.Agency == origin.Agency && destiny.AccountNumber == origin.AccountNumber)
      throw new SelfTransactionException();
  }

  private async Task<bool> IsDuplicatedPayment(PaymentsIdempotenceKey key)
  {
    Payments? recentPayment = await _repository
      .FindRecentPaymentByIdempotenceKey(key, TOLERANCE_DUPLICATE_PAYMENTS_SECONDS);

    return recentPayment is not null;
  }

  public async Task FinishPayment(FinishPaymentsDTO dto, int id)
  {
    await _repository.FinishPaymentAsync(dto.Status, id);
  }
}