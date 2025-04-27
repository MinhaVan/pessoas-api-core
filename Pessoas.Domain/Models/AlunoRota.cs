using System.Collections.Generic;

namespace Aluno.Core.Domain.Models;

public class AlunoRota : Entity
{
    public int AlunoId { get; set; }
    public int RotaId { get; set; }
    //
    public virtual Aluno Aluno { get; set; }
}