using System.Threading.Tasks;
using AutoMapper;
using Pessoas.Core.Domain.Enums;
using Pessoas.Core.Domain.Interfaces.Repository;
using Pessoas.Core.Domain.Interfaces.Services;
using Pessoas.Core.Domain.ViewModels;
using Pessoas.Core.Domain.Models;
using Pessoas.Core.Domain.ViewModels.Motorista;
using Pessoas.Core.Domain.Interfaces.APIs;
using Pessoas.Core.Application.Exceptions;
using System;

namespace Pessoas.Core.Application.Implementations;

public class MotoristaService : IMotoristaService
{
    private readonly IMapper _mapper;
    private readonly IAuthApi _authApi;
    private readonly IBaseRepository<Motorista> _motoristaRepository;
    public MotoristaService(
        IMapper mapper,
        IAuthApi authApi,
        IBaseRepository<Motorista> motoristaRepository)
    {
        _motoristaRepository = motoristaRepository;
        _authApi = authApi;
        _mapper = mapper;
    }

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

    public async Task<MotoristaViewModel> ObterAsync(int motoristaId)
    {
        var motorista = await _motoristaRepository.BuscarUmAsync(x => x.Id == motoristaId);
        var usuarioResponse = await ObterUsuarioPorIdAsync(motorista.UsuarioId);

        var dto = _mapper.Map<MotoristaViewModel>(motorista);
        dto.PrimeiroNome = usuarioResponse.PrimeiroNome;
        dto.UltimoNome = usuarioResponse.UltimoNome;

        return dto;
    }

    public async Task<MotoristaViewModel> ObterPorUsuarioIdAsync(int usuarioId)
    {
        var motorista = await _motoristaRepository.BuscarUmAsync(x => x.UsuarioId == usuarioId);
        var usuarioResponse = await ObterUsuarioPorIdAsync(motorista.UsuarioId);

        var dto = _mapper.Map<MotoristaViewModel>(motorista);
        dto.PrimeiroNome = usuarioResponse.PrimeiroNome;
        dto.UltimoNome = usuarioResponse.UltimoNome;

        return dto;
    }

    private async Task<UsuarioViewModel> ObterUsuarioPorIdAsync(int usuarioId)
    {
        var usuarioResponse = await _authApi.ObterUsuarioPorIdAsync(usuarioId);
        if (usuarioResponse.Sucesso == false || usuarioResponse.Data == null)
            throw new Exception("Usuário para o motorista não encontrado!");

        return usuarioResponse.Data;
    }
}