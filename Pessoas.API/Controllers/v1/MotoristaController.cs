using System.Threading.Tasks;
using Pessoas.Core.Domain.Interfaces.Services;
using Pessoas.Core.Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Pessoas.Core.API.Controllers.v1;

[ApiController]
[Route("v1/[controller]")]
[Authorize("Bearer")]
public class MotoristaController : BaseController
{
    private readonly IMotoristaService _motoristaService;
    public MotoristaController(IMotoristaService motoristaService)
    {
        _motoristaService = motoristaService;
    }

    [HttpGet("{motoristaId}")]
    public async Task<IActionResult> ObterAsync([FromRoute] int motoristaId)
    {
        var response = await _motoristaService.ObterAsync(motoristaId);
        return Success(response);
    }

    [HttpGet("Usuario/{usuarioId}")]
    public async Task<IActionResult> ObterPorUsuarioIdAsync([FromRoute] int usuarioId)
    {
        var response = await _motoristaService.ObterPorUsuarioIdAsync(usuarioId);
        return Success(response);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> AdicionarAsync([FromBody] MotoristaNovoViewModel request)
    {
        await _motoristaService.AdicionarAsync(request);
        return Success();
    }

    [HttpPut]
    public async Task<IActionResult> AtualizarAsync([FromBody] MotoristaAtualizarViewModel request)
    {
        await _motoristaService.AtualizarAsync(request);
        return Success();
    }

    [HttpDelete("{motoristaId}")]
    public async Task<IActionResult> DeletarAsync([FromRoute] int motoristaId)
    {
        await _motoristaService.DeletarAsync(motoristaId);
        return Success();
    }
}