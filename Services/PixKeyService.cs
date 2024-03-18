using System.Net.Mail;
using System.Text.RegularExpressions;
using PixHub.Dtos;
using PixHub.Exceptions;
using PixHub.Models;
using PixHub.Repositories;

namespace PixHub.Services;

public partial class PixKeyService(
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

    if (await _repository.ExistsPixKeyAsync(dto.GetKeyValue())) throw new PixKeyAlreadyExistsException();

    User user = await _userService.FindByCpfWithAccountsThenIncludesPixKeys(dto.GetCpfUser());

    ValidatePixKeyByType(dto.Key, user.CPF);

    if (user.PaymentProviderAccounts is not null)
    {
      ValidateTotalPixKeysLimit(user.PaymentProviderAccounts);
    }

    PaymentProviderAccount? account = user.PaymentProviderAccounts?
      .FirstOrDefault(a =>
        a.Agency == dto.GetAgency() &&
        a.AccountNumber == dto.GetAccountNumber() &&
        a.PaymentProviderId == paymentProvider.Id);

    if (account is not null) ValidateProviderPixKeysLimit(account.PixKeys);
    else account = await _accountService.CreateAsync(dto.Account, user.Id, paymentProvider.Id);

    PixKey pixKey = dto.ToEntity(account!.Id);
    return await _repository.CreateAsync(pixKey);
  }

  private static void ValidateProviderPixKeysLimit(ICollection<PixKey>? pixKeys)
  {
    if (pixKeys is not null && pixKeys.Count >= 5) throw new ProviderPixKeyLimitException();
  }

  private static void ValidateTotalPixKeysLimit(ICollection<PaymentProviderAccount> accounts)
  {
    int totalPixKeyCount = 0;
    foreach (var acc in accounts)
    {
      totalPixKeyCount += acc.PixKeys?.Count ?? 0;
    }
    if (totalPixKeyCount >= 20) throw new TotalPixKeyLimitException();
  }

  private static void ValidatePixKeyByType(KeyDTO key, string cpf)
  {
    switch (key.Type)
    {
      case "CPF":
        if (key.Value != cpf) throw new InvalidCpfPixKeyException();
        break;
      case "Email":
        if (!MailAddress.TryCreate(key.Value, out var _)) throw new InvalidEmailException();
        break;
      case "Phone":
        var regexPhone = PhoneRegex();
        if (!regexPhone.IsMatch(key.Value)) throw new InvalidPhoneException();
        break;
    }
  }

  [GeneratedRegex(@"^(\d{11})$")]
  private static partial Regex PhoneRegex();


  public async Task<OutputPixKeyDTO> FindPixKey(string type, string value, string token)
  {
    PaymentProvider paymentProvider = await _paymentProviderService.FindByTokenAsync(token);

    PixKey pixKey = await _repository.FindWithAccountAndUserAsync(type, value) ??
      throw new PixKeyNotFoundException();

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
    return await _repository.FindWithAccountAndProviderAsync(type, value) ??
      throw new PixKeyNotFoundException();
  }
}