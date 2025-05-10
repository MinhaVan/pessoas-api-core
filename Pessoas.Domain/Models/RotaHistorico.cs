using System;
using Pessoas.Core.Domain.Enums;

namespace Pessoas.Core.Domain.Models;

public class RotaHistorico : Entity
{
    public int RotaId { get; set; }
    public TipoRotaEnum TipoRota { get; set; }
    public DateTime DataRealizacao { get; set; }
    public DateTime? DataFim { get; set; }
    public bool EmAndamento { get; set; }
}