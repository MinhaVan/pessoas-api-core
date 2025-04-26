using System;

namespace Aluno.Core.Domain.Models;

public class AjusteAlunoRota : Entity
{
    public int AlunoId { get; set; }
    public int RotaId { get; set; }
    public int? NovoEnderecoPartidaId { get; set; }
    public int? NovoEnderecoDestinoId { get; set; }
    public int? NovoEnderecoRetornoId { get; set; }
    public DateTime Data { get; set; }
}
