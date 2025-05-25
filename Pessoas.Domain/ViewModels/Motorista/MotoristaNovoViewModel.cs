using System;
using Pessoas.Core.Domain.Enums;

namespace Pessoas.Core.Domain.ViewModels;

public class MotoristaNovoViewModel
{
    public int UsuarioId { get; set; }
    public string CNH { get; set; }
    public DateTime Vencimento { get; set; }
    public TipoCNHEnum TipoCNH { get; set; }
    public string Foto { get; set; }
}