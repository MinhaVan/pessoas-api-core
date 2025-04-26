using System.Threading.Tasks;
using Aluno.Core.Domain.ViewModels.Rota;

namespace Aluno.Core.Domain.Interfaces.APIs;

public interface IRouterAPI
{
    Task<RotaViewModel> ObterRotaPorIdAsync(int rotaId);
}