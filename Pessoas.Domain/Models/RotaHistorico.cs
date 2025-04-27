using System;
using Aluno.Core.Domain.Enums;

namespace Aluno.Core.Domain.Models;

public class RotaHistorico : Entity
{
    public int RotaId { get; set; }
    public TipoRotaEnum TipoRota { get; set; }
    public DateTime DataRealizacao { get; set; }
    public DateTime? DataFim { get; set; }
    public bool EmAndamento { get; set; }
}