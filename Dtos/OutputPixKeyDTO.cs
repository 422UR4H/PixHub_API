namespace PixHub.Dtos;

public class OutputPixKeyDTO(KeyDTO keyDTO, OutputUserDTO user, OutputAccountDTO account)
{
  public KeyDTO Key { get; } = keyDTO;
  public OutputUserDTO User { get; } = user;
  public OutputAccountDTO Account { get; } = account;
}