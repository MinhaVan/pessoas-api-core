using System.Collections.Generic;
using System.Threading.Tasks;
using Pessoas.Core.Domain.ViewModels;

namespace Pessoas.Core.Domain.Interfaces.Services;

public interface IAlunoService
{
    Task AdicionarAsync(int responsavelId, AlunoAdicionarViewModel Alunos);
    Task AtualizarAsync(int responsavelId, AlunoViewModel Alunos);
    Task DeletarAsync(int responsavelId, int AlunoId);
    Task<IList<AlunoViewModel>> ObterTodos(int responsavelId);
    Task<IList<AlunoViewModel>> ObterAluno(int responsavelId, int AlunoId);
    Task<List<AlunoViewModel>> ObterAlunosPorFiltro(int rotaId, string filtro);
    Task<IList<AlunoViewModel>> ObterAlunosAsync(List<int> alunosIds);
}