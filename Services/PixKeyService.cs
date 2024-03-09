using PixHub.Dtos;
using PixHub.Exceptions;
using PixHub.Models;
using PixHub.Repositories;

namespace PixHub.Services;

public class PixKeyService(
  PixKeyRepository repository,
  PaymentProviderAccountService accountService,
  PaymentProviderService paymentProviderService,
  UserService userService)
{
  readonly PixKeyRepository _repository = repository;
  readonly UserService _userService = userService;
  readonly PaymentProviderAccountService _accountService = accountService;
  readonly PaymentProviderService _paymentProviderService = paymentProviderService;

  public async Task<PixKey> CreatePixKey(CreatePixKeyDTO dto, string token)
  {
    PaymentProvider paymentProvider = await _paymentProviderService.FindByTokenAsync(token) ??
      throw new UnauthorizedProviderException();

    // TODO: refactor to FindByCpfWithPaymentProvider
    User user = await _userService.FindByCpfAsync(dto.User.Cpf) ?? throw new UserNotFoundException();

    if (await _repository.ExistsPixKeyAsync(dto.Key.Value)) throw new PixKeyAlreadyExistsException();

    if (dto.Key.Type == "CPF" && dto.Key.Value != user.CPF) throw new InvalidCpfPixKeyException();

    AccountDTO accountDTO = dto.Account;

    PaymentProviderAccount? account = user.PaymentProviderAccounts?
      .FirstOrDefault(a =>
        a.AccountNumber.Equals(accountDTO.Number) &&
        a.Agency.Equals(accountDTO.Agency) &&
        a.PaymentProviderId.Equals(paymentProvider.Id));

    if (account is not null)
      await ValidatePixKeysLimit(account.Id);
    else
      account = await _accountService.CreateAsync(accountDTO, user.Id, paymentProvider.Id);

    PixKey pixKey = dto.ToEntity(account.Id);
    return await _repository.CreateAsync(pixKey) ?? throw new PixKeyPersistenceDatabaseException();
  }

  public async Task ValidatePixKeysLimit(int accountId)
  {
    int providerPixKeyCount = await _repository.CountAsync(accountId);
    if (providerPixKeyCount >= 5) throw new ProviderPixKeyLimitException();

    int totalPixKeyCount = await _repository.CountAsync();
    if (totalPixKeyCount >= 20) throw new TotalPixKeyLimitException();
  }
}