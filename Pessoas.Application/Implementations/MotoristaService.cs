using System.Threading.Tasks;
using AutoMapper;
using Pessoas.Core.Domain.Enums;
using Pessoas.Core.Domain.Interfaces.Repository;
using Pessoas.Core.Domain.Interfaces.Services;
using Pessoas.Core.Domain.ViewModels;
using Pessoas.Core.Domain.Models;
using Pessoas.Core.Domain.Interfaces.APIs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pessoas.Core.Application.Implementations;

public class MotoristaService(
    IMapper _mapper,
    IUserContext _userContext,
    IAuthApi _authApi,
    IBaseRepository<Motorista> _motoristaRepository) : IMotoristaService
{
    public async Task AdicionarAsync(MotoristaNovoViewModel usuarioNovoViewModel)
    {
        var model = _mapper.Map<Motorista>(usuarioNovoViewModel);
        await _motoristaRepository.AdicionarAsync(model);
    }

    public async Task AtualizarAsync(MotoristaAtualizarViewModel usuarioAtualizarViewModel)
    {
        await _authApi.AtualizarAsync(usuarioAtualizarViewModel);

        var motorista = await _motoristaRepository.BuscarUmAsync(x => x.UsuarioId == usuarioAtualizarViewModel.Id);
        motorista.CNH = usuarioAtualizarViewModel.CNH;
        motorista.Vencimento = usuarioAtualizarViewModel.Vencimento;
        motorista.TipoCNH = usuarioAtualizarViewModel.TipoCNH;
        motorista.Foto = usuarioAtualizarViewModel.Foto;
        await _motoristaRepository.AtualizarAsync(motorista);
    }

    public async Task DeletarAsync(int motoristaId)
    {
        await _motoristaRepository.RemoverAsync(motoristaId);
    }

    public async Task<List<MotoristaViewModel>> ObterTodosAsync(bool completarDadosDoUsuario)
    {
        var motoristas = await _motoristaRepository.BuscarAsync(x => x.Status == StatusEntityEnum.Ativo);
        var dto = _mapper.Map<List<MotoristaViewModel>>(motoristas);

        if (dto is null)
            return default;

        if (completarDadosDoUsuario)
        {
            var tasks = dto.Select(async x =>
            {
                var usuarioResponse = await ObterUsuarioPorIdAsync(x.UsuarioId.Value, obterDadosMotorista: false);
                x.PrimeiroNome = usuarioResponse.PrimeiroNome;
                x.UltimoNome = usuarioResponse.UltimoNome;
                x.CPF = usuarioResponse.CPF;
                x.Contato = usuarioResponse.Contato;
                x.Email = usuarioResponse.Email;
                x.Perfil = usuarioResponse.Perfil;
                x.PlanoId = usuarioResponse.PlanoId;
                x.UsuarioValidado = usuarioResponse.UsuarioValidado;
                x.EnderecoPrincipalId = usuarioResponse.EnderecoPrincipalId;
                x.Senha = string.Empty;
                x.EmpresaId = usuarioResponse.EmpresaId;
            });

            await Task.WhenAll(tasks);
        }

        return dto;
    }

    public async Task<MotoristaViewModel> ObterAsync(int motoristaId, bool completarDadosDoUsuario)
    {
        var motorista = await _motoristaRepository.BuscarUmAsync(x => x.Id == motoristaId);
        var dto = _mapper.Map<MotoristaViewModel>(motorista);

        if (dto is null)
            return default;

        if (completarDadosDoUsuario)
        {
            var usuarioResponse = await ObterUsuarioPorIdAsync(motorista.UsuarioId);
            dto.PrimeiroNome = usuarioResponse.PrimeiroNome;
            dto.UltimoNome = usuarioResponse.UltimoNome;
        }

        return dto;
    }

    public async Task<MotoristaViewModel> ObterPorUsuarioIdAsync(int usuarioId, bool completarDadosDoUsuario)
    {
        var motorista = await _motoristaRepository.BuscarUmAsync(x => x.UsuarioId == usuarioId);
        var dto = _mapper.Map<MotoristaViewModel>(motorista);

        if (completarDadosDoUsuario)
        {
            var usuarioResponse = await ObterUsuarioPorIdAsync(motorista.UsuarioId);
            dto.PrimeiroNome = usuarioResponse.PrimeiroNome;
            dto.UltimoNome = usuarioResponse.UltimoNome;
        }

        return dto;
    }

    private async Task<UsuarioViewModel> ObterUsuarioPorIdAsync(int usuarioId, bool obterDadosMotorista = true)
    {
        var usuarioResponse = await _authApi.ObterUsuarioPorIdAsync(_userContext.Token, usuarioId, obterDadosMotorista);
        if (usuarioResponse.Sucesso == false || usuarioResponse.Data == null)
            throw new Exception("Usuário para o motorista não encontrado!");

        return usuarioResponse.Data;
    }
}