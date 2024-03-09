using PixHub.Exceptions;

namespace PixHub.Utils;

public static class CpfUtil
{
  public static string ToMasked(string cpf)
  {
    if (string.IsNullOrEmpty(cpf) || cpf.Length != 11)
    {
      throw new InvalidCpfException();
    }

    return string.Concat(cpf.AsSpan(0, 3), ".XXX.XXX-", cpf.AsSpan(9, 2));
  }
}