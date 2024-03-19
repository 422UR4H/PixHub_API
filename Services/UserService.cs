using PixHub.Exceptions;
using PixHub.Models;
using PixHub.Repositories;

namespace PixHub.Services;

public class UserService(UserRepository repository)
{
  readonly UserRepository _repository = repository;

  public async Task<User> FindByCpfWithAccountsThenIncludesPixKeys(string cpf)
  {
    return await _repository.FindByCpfWithAccountsThenIncludesPixKeysAsync(cpf) ??
      throw new UserNotFoundException();
  }
}