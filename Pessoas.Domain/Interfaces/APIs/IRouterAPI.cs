using System.Collections.Generic;
using System.Threading.Tasks;
using Aluno.Core.Domain.ViewModels;
using Aluno.Core.Domain.ViewModels.Rota;

namespace Aluno.Core.Domain.Interfaces.APIs;

public interface IRouterAPI
{
    Task<RotaViewModel> ObterRotaPorIdAsync(int rotaId);
    //
    Task AdicionarEnderecoAsync(EnderecoAdicionarViewModel enderecoAdicionarViewModel);
    Task AtualizarEnderecoAsync(int id, EnderecoAtualizarViewModel enderecoAtualizarViewModel);
    Task DeletarEnderecoAsync(int id);
    Task<EnderecoViewModel> ObterEnderecoPorIdAsync(int id);
    Task<IEnumerable<EnderecoViewModel>> ObterTodosEnderecosAsync();
    Task<List<EnderecoViewModel>> ObterEnderecoPorIdAsync(List<int> ids);
}