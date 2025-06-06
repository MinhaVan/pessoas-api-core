using Aluno.Core.Domain.Enums;

namespace Aluno.Core.Domain.ViewModels;

public class AlunoRotaViewModel
{
    public int AlunoId { get; set; }
    public int RotaId { get; set; }
    public StatusEntityEnum Status { get; set; }
}