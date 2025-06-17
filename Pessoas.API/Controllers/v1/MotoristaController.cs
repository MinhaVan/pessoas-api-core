using System.Threading.Tasks;
using Pessoas.Core.Domain.Interfaces.Services;
using Pessoas.Core.Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using Pessoas.Core.Domain.Enums;
using Pessoas.Domain.ViewModels.Motorista;

namespace Pessoas.Core.API.Controllers.v1;

[ApiController]
[Route("v1/[controller]")]
[Authorize("Bearer")]
public class MotoristaController(IMotoristaService _motoristaService) : BaseController
{
    [HttpGet("CNH")]
    public IActionResult ObterTipoCNHAsync()
    {
        var tipoCNH = new List<TipoCNHViewModel>();
        foreach (var value in Enum.GetValues(typeof(TipoCNHEnum)))
        {
            tipoCNH.Add(new TipoCNHViewModel
            {
                Id = (int)value,
                Nome = value.ToString()
            });
        }
        return Success(tipoCNH);
    }

    [HttpGet]
    public async Task<IActionResult> ObterAsync([FromQuery] bool completarDadosDoUsuario = false, [FromQuery] bool adicionarDeletados = false)
    {
        var response = await _motoristaService.ObterTodosAsync(completarDadosDoUsuario, adicionarDeletados);
        return Success(response);
    }

    [HttpGet("{motoristaId}")]
    public async Task<IActionResult> ObterAsync([FromRoute] int motoristaId, [FromQuery] bool completarDadosDoUsuario = false)
    {
        var response = await _motoristaService.ObterAsync(motoristaId, completarDadosDoUsuario);
        return Success(response);
    }

    [HttpGet("Usuario/{usuarioId}")]
    public async Task<IActionResult> ObterPorUsuarioIdAsync([FromRoute] int usuarioId, [FromQuery] bool completarDadosDoUsuario = false)
    {
        var response = await _motoristaService.ObterPorUsuarioIdAsync(usuarioId, completarDadosDoUsuario);
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