using Pessoas.Core.Domain.Enums;

namespace Pessoas.Core.Domain.ViewModels.Motorista;

public class UsuarioMotoristaNovoViewModel
{
    public string CPF { get; set; }
    public string Senha { get; set; }
    public string Contato { get; set; }
    public string Email { get; set; }
    public string PrimeiroNome { get; set; }
    public string UltimoNome { get; set; }
}