using System;
using Aluno.Core.Domain.Enums;

namespace Aluno.Core.Domain.Models;

public class Motorista : Entity
{
    public int UsuarioId { get; set; }
    public string CNH { get; set; }
    public DateTime Vencimento { get; set; }
    public TipoCNHEnum TipoCNH { get; set; }
    public string Foto { get; set; }
}