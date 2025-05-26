using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pessoas.Core.Domain.ViewModels;
using Pessoas.Core.Domain.Interfaces.Services;
using Pessoas.Core.Domain.Interfaces.Repository;
using System.Collections.Generic;

namespace Pessoas.Core.API.Controllers.v1;

[ApiController]
[Route("v1/[controller]")]
[Authorize("Bearer")]
public class AlunoController : BaseController
{
    private readonly IAlunoService _alunoService;
    private readonly IUserContext _userContext;
    public AlunoController(IAlunoService alunoService, IUserContext userContext)
    {
        _userContext = userContext;
        _alunoService = alunoService;
    }

    [HttpGet]
    public async Task<IActionResult> ObterAlunos()
    {
        var responsavelId = _userContext.UserId;
        return Success(await _alunoService.ObterTodos(responsavelId));
    }

    [HttpGet("{alunosIds}")]
    public async Task<IActionResult> ObterAlunosAsync([FromRoute] List<int> alunosIds)
    {
        var alunos = await _alunoService.ObterAlunosAsync(alunosIds);
        return Success(alunos);
    }

    [HttpGet("buscar")]
    public async Task<IActionResult> ObterAlunosPorFiltro([FromQuery] string filtro, [FromQuery] int? rotaId = 0)
    {
        return Success(await _alunoService.ObterAlunosPorFiltro(rotaId.Value, filtro));
    }

    [HttpPost]
    public async Task<IActionResult> AdicionarAluno(AlunoAdicionarViewModel aluno)
    {
        var responsavelId = _userContext.UserId;
        await _alunoService.AdicionarAsync(responsavelId, aluno);
        return Success();
    }

    [HttpPut]
    public async Task<IActionResult> AtualizarAluno(AlunoViewModel aluno)
    {
        var responsavelId = _userContext.UserId;
        await _alunoService.AtualizarAsync(responsavelId, aluno);
        return Success();
    }

    [HttpDelete("{alunoId}")]
    public async Task<IActionResult> DeletarAluno(int alunoId)
    {
        var responsavelId = _userContext.UserId;
        await _alunoService.DeletarAsync(responsavelId, alunoId);
        return Success();
    }
}
