using System.Text.RegularExpressions;
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
    PaymentProvider paymentProvider = await _paymentProviderService.FindByTokenAsync(token);

    User user = await _userService.FindByCpfWithPaymentProviderAccounts(dto.User.Cpf);

    if (await _repository.ExistsPixKeyAsync(dto.Key.Value)) throw new PixKeyAlreadyExistsException();

    ValidatePixKeyByType(dto.Key, user.CPF);

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

  public async Task ValidateIfExistsPixKey(string value)
  {
    if (await _repository.ExistsPixKeyAsync(value)) throw new PixKeyAlreadyExistsException();
  }

  public async Task ValidatePixKeysLimit(int accountId)
  {
    int providerPixKeyCount = await _repository.CountAsync(accountId);
    if (providerPixKeyCount >= 5) throw new ProviderPixKeyLimitException();

    int totalPixKeyCount = await _repository.CountAsync();
    if (totalPixKeyCount >= 20) throw new TotalPixKeyLimitException();
  }

  public void ValidatePixKeyByType(KeyDTO key, string cpf)
  {
    switch (key.Type)
    {
      case "CPF":
        if (key.Value != cpf) throw new InvalidCpfPixKeyException();
        break;
      case "Email":
        var regexEmail = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        if (!regexEmail.IsMatch(key.Value))
        {
          throw new InvalidEmailException();
        }
        break;
      case "Phone":
        var regexPhone = new Regex(@"^(\d{11})$");
        if (!regexPhone.IsMatch(key.Value))
        {
          throw new InvalidPhoneException();
        }
        break;
    }
  }

  public async Task<OutputPixKeyDTO> FindPixKey(string type, string value, string token)
  {
    PaymentProvider paymentProvider = await _paymentProviderService.FindByTokenAsync(token);

    PixKey pixKey = await _repository.FindAsync(type, value) ?? throw new PixKeyNotFoundException();

    PaymentProviderAccount account = pixKey.PaymentProviderAccount ??
      throw new PaymentProviderAccountNotFoundException();

    User user = account.User ?? throw new UserNotFoundException();

    KeyDTO keyDTO = new(pixKey);
    OutputUserDTO userDTO = new(user);
    OutputAccountDTO accountDTO = new(account, paymentProvider);

    return new OutputPixKeyDTO(keyDTO, userDTO, accountDTO);
  }

  public async Task<PixKey> FindWithAccountAndProvider(string type, string value)
  {
    return await _repository.FindWithAccountAndProviderAsync(type, value)
      ?? throw new PixKeyNotFoundException();
  }
}