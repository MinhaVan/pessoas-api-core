using System.Threading.Tasks;
using Pessoas.Core.Domain.ViewModels;

namespace Pessoas.Core.Domain.Interfaces.Services;

public interface IMotoristaService
{
    Task AdicionarAsync(MotoristaNovoViewModel request);
    Task AtualizarAsync(MotoristaAtualizarViewModel request);
    Task DeletarAsync(int motoristaId);
    Task<MotoristaViewModel> ObterAsync(int motoristaId);
    Task<MotoristaViewModel> ObterPorUsuarioIdAsync(int usuarioId);
}