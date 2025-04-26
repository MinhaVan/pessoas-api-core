using System;
using Aluno.Core.Domain.Enums;
using Aluno.Core.Domain.ViewModels.Motorista;

namespace Aluno.Core.Domain.ViewModels;

public class MotoristaNovoViewModel : UsuarioNovoViewModel
{
    public string CNH { get; set; }
    public DateTime Vencimento { get; set; }
    public TipoCNHEnum TipoCNH { get; set; }
    public string Foto { get; set; }
}