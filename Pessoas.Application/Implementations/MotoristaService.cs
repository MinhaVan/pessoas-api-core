using System.Threading.Tasks;
using AutoMapper;
using Aluno.Core.Domain.Enums;
using Aluno.Core.Domain.Interfaces.Repository;
using Aluno.Core.Domain.Interfaces.Services;
using Aluno.Core.Domain.ViewModels;
using Aluno.Core.Domain.Models;
using Aluno.Core.Domain.ViewModels.Motorista;
using Aluno.Core.Domain.Interfaces.APIs;
using Aluno.Core.Service.Exceptions;

namespace Aluno.Core.Service.Implementations;

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
        usuarioNovoViewModel.Perfil = PerfilEnum.Motorista;
        var response = await _authApi.RegistrarAsync(_mapper.Map<UsuarioNovoViewModel>(usuarioNovoViewModel));
        if (!response.Sucesso)
            throw new BusinessRuleException(response.Mensagem);

        var model = _mapper.Map<Motorista>(usuarioNovoViewModel);
        model.UsuarioId = response.Data.Id;
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
        return _mapper.Map<MotoristaViewModel>(motorista);
    }

    public async Task<MotoristaViewModel> ObterPorUsuarioIdAsync(int usuarioId)
    {
        var motorista = await _motoristaRepository.BuscarUmAsync(x => x.UsuarioId == usuarioId);
        return _mapper.Map<MotoristaViewModel>(motorista);
    }
}