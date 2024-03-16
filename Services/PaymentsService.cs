using PixHub.Dtos;
using PixHub.Exceptions;
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

  public async Task<Payments?> CreatePayment(CreatePaymentDTO paymentDTO, string token)
  {
    PaymentProvider originProvider = await _paymentProviderService.FindByTokenAsync(token);

    // TODO: refactor object chain
    AccountDTO originAccountDTO = paymentDTO.Origin.Account;
    PaymentProviderAccount originAccount = await _accountService.FindByUserCpf(
      paymentDTO.Origin.User.Cpf,
      originAccountDTO.Agency,
      originAccountDTO.Number);

    // TODO: refactor object chain
    PixKey pixKey = await _pixKeyService
      .FindWithAccountAndProvider(paymentDTO.Destiny.Key.Type, paymentDTO.Destiny.Key.Value);

    PaymentProviderAccount destinyAccount = pixKey?.PaymentProviderAccount ??
      throw new PaymentProviderAccountNotFoundException("Destiny Payment Provider Account not found!");

    PaymentProvider destinyProvider = destinyAccount?.PaymentProvider ??
      throw new PaymentProviderNotFoundException();

    ValidateSelfTransaction(originAccount, destinyAccount);

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

  public async Task FinishPayment(FinishPaymentsDTO dto, int id)
  {
    await _repository.FinishPaymentAsync(dto.Status, id);
  }
}