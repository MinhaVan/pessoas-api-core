using Pessoas.Core.Domain.Enums;

namespace Pessoas.Core.Domain.ViewModels;

public class AlunoRotaViewModel
{
    public int AlunoId { get; set; }
    public int RotaId { get; set; }
    public StatusEntityEnum Status { get; set; }
}