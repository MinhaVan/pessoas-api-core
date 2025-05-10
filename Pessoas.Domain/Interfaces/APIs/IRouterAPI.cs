using System.Collections.Generic;
using System.Threading.Tasks;
using Pessoas.Core.Domain.ViewModels;
using Pessoas.Core.Domain.ViewModels.Rota;

namespace Pessoas.Core.Domain.Interfaces.APIs;

public interface IRouterAPI
{
    Task<RotaViewModel> ObterRotaPorIdAsync(int rotaId);
    //
    Task<BaseResponse<List<AlunoRotaViewModel>>> ObterRotasPorAlunoAsync(int rotaId, int? alunoId = null);
    Task<BaseResponse<object>> AtualizarAlunoRotaAsync(AlunoRotaViewModel alunoRotaAtualizarViewModel);
    Task<BaseResponse<object>> AdicionarAlunoRotaAsync(AlunoRotaViewModel alunoRotaAdicionarViewModel);
    //
    Task AdicionarEnderecoAsync(EnderecoAdicionarViewModel enderecoAdicionarViewModel);
    Task AtualizarEnderecoAsync(int id, EnderecoAtualizarViewModel enderecoAtualizarViewModel);
    Task DeletarEnderecoAsync(int id);
    Task<EnderecoViewModel> ObterEnderecoPorIdAsync(int id);
    Task<IEnumerable<EnderecoViewModel>> ObterTodosEnderecosAsync();
    Task<List<EnderecoViewModel>> ObterEnderecoPorIdAsync(List<int> ids);
}