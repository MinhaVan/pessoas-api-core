using System;
using Pessoas.Core.Domain.Enums;

namespace Pessoas.Core.Domain.Models;

public abstract class Entity
{
    public int Id { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime DataAlteracao { get; set; }
    public StatusEntityEnum Status { get; set; }
}