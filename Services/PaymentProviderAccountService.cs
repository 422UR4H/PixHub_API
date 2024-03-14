using PixHub.Dtos;
using PixHub.Exceptions;
using PixHub.Models;
using PixHub.Repositories;

namespace PixHub.Services;

public class PaymentProviderAccountService(PaymentProviderAccountRepository repository)
{
  readonly PaymentProviderAccountRepository _repository = repository;

  public async Task<PaymentProviderAccount> CreateAsync(AccountDTO dto, int userId, int paymentProviderId)
  {
    if (await _repository.FindByAccountNumberAsync(dto.Number) is not null)
    {
      throw new PaymentProviderAccountAlreadyExistsException();
    }

    PaymentProviderAccount paymentProviderAccount = new(dto.Number, dto.Agency, userId, paymentProviderId);
    PaymentProviderAccount response = await _repository.CreateAsync(paymentProviderAccount);
    return response;
  }

  public async Task<PaymentProviderAccount> FindByUserCpf(string cpf, string agency, string number)
  {
    return await _repository.FindByUserCpfAsync(cpf, agency, number) ??
      throw new PaymentProviderAccountNotFoundException();
  }
}