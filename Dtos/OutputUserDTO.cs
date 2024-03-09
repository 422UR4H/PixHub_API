using PixHub.Models;
using PixHub.Utils;

namespace PixHub.Dtos;

public class OutputUserDTO(string name, string cpf)
{
    public string Name { get; } = name;
    public string MaskedCpf { get; } = CpfUtil.ToMasked(cpf);

    public OutputUserDTO(User user) : this(user.Name, user.CPF) { }
}