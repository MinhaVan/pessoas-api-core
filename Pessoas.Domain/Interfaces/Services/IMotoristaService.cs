using System.Collections.Generic;
using System.Threading.Tasks;
using Pessoas.Core.Domain.ViewModels;
using Pessoas.Domain.ViewModels.Motorista;

namespace Pessoas.Core.Domain.Interfaces.Services;

public interface IMotoristaService
{
    Task AdicionarAsync(UsuarioNovoViewModel request);
    Task AtualizarAsync(MotoristaAtualizarViewModel request);
    Task DeletarAsync(int motoristaId);
    Task<MotoristaViewModel> ObterAsync(int motoristaId, bool completarDadosDoUsuario);
    Task<List<MotoristaViewModel>> ObterTodosAsync(bool completarDadosDoUsuario, bool adicionarDeletados = false);
    Task<MotoristaViewModel> ObterPorUsuarioIdAsync(int usuarioId, bool completarDadosDoUsuario);
}