using System;
using System.Collections.Generic;
using Pessoas.Core.Domain.Enums;
using Pessoas.Core.Domain.ViewModels.Motorista;

namespace Pessoas.Core.Domain.ViewModels;

public class MotoristaViewModel : UsuarioViewModel
{
    public string CNH { get; set; }
    public DateTime Vencimento { get; set; }
    public TipoCNHEnum TipoCNH { get; set; }
    public string Foto { get; set; }
    public List<MotoristaRotaViewModel> MotoristaRotas { get; set; } = new();
}