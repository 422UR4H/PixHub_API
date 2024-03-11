using PixHub.Exceptions;
using PixHub.Models;
using PixHub.Repositories;

namespace PixHub.Services;

public class UserService(UserRepository repository)
{
  readonly UserRepository _repository = repository;

  public async Task<User> FindByCpfWithPaymentProviderAccount(string cpf)
  {
    return await _repository.FindByCpfAsync(cpf) ?? throw new UserNotFoundException();
  }
}