using PixHub.Dtos;
using PixHub.Exceptions;
using PixHub.IdempotenceKeys;
using PixHub.Middlewares;
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

  const int TOLERANCE_DUPLICATE_PAYMENTS_SECONDS = 30;

  public async Task<OutputPaymentDTO?> CreatePayment(CreatePaymentDTO paymentDTO, string token)
  {
    PaymentProvider originProvider = await _paymentProviderService.FindByTokenAsync(token);

    PaymentProviderAccount originAccount = await _accountService.FindByUserCpf(
      paymentDTO.GetCpfUser(),
      paymentDTO.GetOriginAgency(),
      paymentDTO.GetOriginAccountNumber());

    ValidationMiddleware.ValidatesRequestIntegrityBy(originProvider, originAccount);

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
      new(payment.Id, payment.TransactionId, paymentDTO, originProvider.Webhook, destinyProvider.Webhook);

    _messageService.SendMessage(transferPaymentDTO, "payments");
    return new OutputPaymentDTO(payment.TransactionId);
  }

  private static void ValidateSelfTransaction(PaymentProviderAccount origin, PaymentProviderAccount destiny)
  {
    if (destiny.Agency == origin.Agency && destiny.AccountNumber == origin.AccountNumber)
    {
      throw new SelfTransactionException();
    }
  }

  private async Task<bool> IsDuplicatedPayment(PaymentsIdempotenceKey key)
  {
    Payments? recentPayment = await _repository
      .FindRecentPaymentByIdempotenceKey(key, TOLERANCE_DUPLICATE_PAYMENTS_SECONDS);

    return recentPayment is not null;
  }

  public async Task FinishPayment(FinishPaymentsDTO dto, int id, Guid transactionId)
  {
    await _repository.FinishPaymentAsync(dto.Status, id, transactionId);
  }
}