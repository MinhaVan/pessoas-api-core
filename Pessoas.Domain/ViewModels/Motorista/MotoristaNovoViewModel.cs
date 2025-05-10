using System;
using Pessoas.Core.Domain.Enums;
using Pessoas.Core.Domain.ViewModels.Motorista;

namespace Pessoas.Core.Domain.ViewModels;

public class MotoristaNovoViewModel : UsuarioNovoViewModel
{
    public string CNH { get; set; }
    public DateTime Vencimento { get; set; }
    public TipoCNHEnum TipoCNH { get; set; }
    public string Foto { get; set; }
}