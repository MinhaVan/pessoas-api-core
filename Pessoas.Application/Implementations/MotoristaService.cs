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
using Pessoas.Core.Domain.ViewModels.Motorista;
using Pessoas.Domain.ViewModels.Motorista;
using Pessoas.Domain.Interfaces.Repositories;
using Pessoas.Domain.Utils;

namespace Pessoas.Core.Application.Implementations;

public class MotoristaService(
    IMapper _mapper,
    IRedisRepository _redisRepository,
    IUserContext _userContext,
    IAuthApi _authApi,
    IBaseRepository<Motorista> _motoristaRepository) : IMotoristaService
{
    public async Task AdicionarAsync(UsuarioNovoViewModel usuarioNovoViewModel)
    {
        var model = _mapper.Map<Motorista>(usuarioNovoViewModel);
        var usuarioMotorista = new UsuarioMotoristaNovoViewModel
        {
            Contato = usuarioNovoViewModel.Contato,
            Email = usuarioNovoViewModel.Email,
            PrimeiroNome = usuarioNovoViewModel.PrimeiroNome,
            UltimoNome = usuarioNovoViewModel.UltimoNome,
            CPF = usuarioNovoViewModel.CPF,
            Senha = usuarioNovoViewModel.Senha,
            EmpresaId = usuarioNovoViewModel.EmpresaId,
        };
        var response = await _authApi.RegistrarAsync(usuarioMotorista);

        model.UsuarioId = response.Data.Id;
        await _motoristaRepository.AdicionarAsync(model);
        await LimparCache();
    }

    public async Task AtualizarAsync(MotoristaAtualizarViewModel usuarioAtualizarViewModel)
    {
        var motorista = await _motoristaRepository.BuscarUmAsync(x => x.Id == usuarioAtualizarViewModel.Id);

        usuarioAtualizarViewModel.Id = motorista.UsuarioId;
        await _authApi.AtualizarAsync(_userContext.Token, usuarioAtualizarViewModel);

        motorista.Status = usuarioAtualizarViewModel.Status;
        motorista.CNH = usuarioAtualizarViewModel.CNH;
        motorista.Vencimento = usuarioAtualizarViewModel.Vencimento;
        motorista.TipoCNH = usuarioAtualizarViewModel.TipoCNH;
        motorista.Foto = usuarioAtualizarViewModel.Foto;
        await _motoristaRepository.AtualizarAsync(motorista);
        await LimparCache();
    }

    public async Task DeletarAsync(int motoristaId)
    {
        await _motoristaRepository.RemoverAsync(motoristaId);
        await LimparCache();
    }

    public async Task<List<MotoristaViewModel>> ObterTodosAsync(bool completarDadosDoUsuario, bool adicionarDeletados = false)
    {
        var chave = string.Format(KeyRedis.Motoristas.TodosMotoristas, adicionarDeletados, completarDadosDoUsuario);
        var motoristas = await _redisRepository.GetAsync<IEnumerable<Motorista>>(chave);

        if (motoristas is null || !motoristas.Any())
        {
            motoristas = await _motoristaRepository.BuscarAsync(x => x.Status == StatusEntityEnum.Ativo || (adicionarDeletados && x.Status == StatusEntityEnum.Deletado));
            if (motoristas is not null && motoristas.Any())
            {
                await _redisRepository.SetAsync(chave, motoristas, durationInMinutes: 60);
            }
        }

        if (motoristas is null || !motoristas.Any())
            return default;

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

        return dto.OrderBy(x => x.Status).ToList();
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

    private async Task LimparCache()
    {
        var tasks = new[]
        {
            _redisRepository.DeleteAsync(string.Format(KeyRedis.Motoristas.TodosMotoristas, false, false)),
            _redisRepository.DeleteAsync(string.Format(KeyRedis.Motoristas.TodosMotoristas, false, true)),
            _redisRepository.DeleteAsync(string.Format(KeyRedis.Motoristas.TodosMotoristas, true, false)),
            _redisRepository.DeleteAsync(string.Format(KeyRedis.Motoristas.TodosMotoristas, true, true))
        };

        await Task.WhenAll(tasks);
    }
}