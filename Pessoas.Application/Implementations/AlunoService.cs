using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Aluno.Core.Domain.Interfaces.Repository;
using Aluno.Core.Domain.Enums;
using Aluno.Core.Domain.Models;
using Aluno.Core.Domain.ViewModels;
using Aluno.Core.Domain.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Aluno.Core.Domain.Interfaces.APIs;

namespace Aluno.Core.Service.Implementations;

public class AlunoService : IAlunoService
{
    private readonly IMapper _mapper;
    private readonly IBaseRepository<Domain.Models.Aluno> _alunoRepository;
    private readonly IBaseRepository<AlunoRota> _alunoRotaRepository;
    private readonly IRouterAPI _routerAPI;
    private readonly IUserContext _userContext;
    public AlunoService(
        IRouterAPI routerAPI,
        IBaseRepository<Domain.Models.Aluno> alunoRepository,
        IBaseRepository<AlunoRota> AlunoRotaRepository,
        IUserContext userContext,
        IMapper map)
    {
        _userContext = userContext;
        _mapper = map;
        _routerAPI = routerAPI;
        _alunoRotaRepository = AlunoRotaRepository;
        _alunoRepository = alunoRepository;
    }

    public async Task AdicionarAsync(int responsavelId, AlunoAdicionarViewModel AlunoVm)
    {
        var aluno = _mapper.Map<Domain.Models.Aluno>(AlunoVm);
        aluno.ResponsavelId = responsavelId;
        aluno.Status = StatusEntityEnum.Ativo;
        aluno.EmpresaId = _userContext.Empresa;

        await _alunoRepository.AdicionarAsync(aluno);
    }

    public async Task AtualizarAsync(int responsavelId, AlunoViewModel AlunoVm)
    {
        var aluno = await _alunoRepository.ObterPorIdAsync(AlunoVm.Id);

        aluno.ResponsavelId = responsavelId;
        aluno.CPF = AlunoVm.CPF;
        aluno.PrimeiroNome = AlunoVm.PrimeiroNome;
        aluno.UltimoNome = AlunoVm.UltimoNome;
        aluno.Contato = AlunoVm.Contato;
        aluno.Email = AlunoVm.Email;
        aluno.EnderecoPartidaId = AlunoVm.EnderecoPartidaId;
        aluno.EnderecoRetornoId = AlunoVm.EnderecoRetornoId;
        aluno.EnderecoDestinoId = AlunoVm.EnderecoDestinoId;

        await _alunoRepository.AtualizarAsync(aluno);
    }

    public async Task DeletarAsync(int responsavelId, int AlunoId)
    {
        var Alunos = await _alunoRepository.BuscarAsync(x => x.ResponsavelId == responsavelId && x.Id == AlunoId);
        if (Alunos is null || !Alunos.Any())
            throw new Exception("Nenhum aluno encontrado para o usuário!");

        var aluno = Alunos.First();
        aluno.Status = StatusEntityEnum.Deletado;
        await _alunoRepository.AtualizarAsync(aluno);
    }

    public async Task<IList<AlunoViewModel>> ObterTodos(int responsavelId)
    {
        var Alunos = await _alunoRepository.BuscarAsync(x => x.ResponsavelId == responsavelId && x.Status == StatusEntityEnum.Ativo);
        return _mapper.Map<List<AlunoViewModel>>(Alunos);
    }

    public async Task<List<AlunoViewModel>> ObterAlunosPorFiltro(int rotaId, string filtro)
    {
        filtro = filtro.Trim().ToLower();

        // Buscar todos os alunos que correspondem ao filtro
        var alunos = await _alunoRepository.BuscarAsync(
            x => (EF.Functions.ILike(x.PrimeiroNome, $"%{filtro}%") ||
                EF.Functions.ILike(x.UltimoNome, $"%{filtro}%") ||
                EF.Functions.ILike(x.PrimeiroNome + " " + x.UltimoNome, $"%{filtro}%") ||
                EF.Functions.ILike(x.Contato, $"%{filtro}%") ||
                EF.Functions.ILike(x.Email, $"%{filtro}%") ||
                EF.Functions.ILike(x.CPF, $"%{filtro}%")) &&
                x.Status == StatusEntityEnum.Ativo &&
                x.EmpresaId == _userContext.Empresa,
            x => x.EnderecoPartida, x => x.EnderecoDestino, x => x.EnderecoRetorno, x => x.AlunoRotas
        );

        // Buscar IDs dos alunos que já estão cadastrados na rota
        var alunosNaRota = await _alunoRotaRepository.BuscarAsync(
            x => x.RotaId == rotaId && x.Status == StatusEntityEnum.Ativo
        );

        var alunosNaRotaIds = alunosNaRota.Select(x => x.AlunoId).ToList();

        // Filtrar alunos que NÃO estão na rota
        var alunosForaDaRota = alunos.Where(x => !alunosNaRotaIds.Contains(x.Id)).ToList();

        return _mapper.Map<List<AlunoViewModel>>(alunosForaDaRota);
    }

    public async Task<IList<AlunoViewModel>> ObterAluno(int responsavelId, int AlunoId)
    {
        var Alunos = await _alunoRepository.BuscarAsync(x => x.ResponsavelId == responsavelId && x.Id == AlunoId);
        return _mapper.Map<List<AlunoViewModel>>(Alunos);
    }

    public async Task VincularRotaAsync(int rotaId, int alunoId)
    {
        if (rotaId < 1 || alunoId < 1)
            return;

        var alunoRota = await _alunoRotaRepository.BuscarUmAsync(x =>
            x.AlunoId == alunoId &&
            x.RotaId == rotaId &&
            x.Status != StatusEntityEnum.Ativo);

        var rotaExistente = await _routerAPI.ObterRotaPorIdAsync(rotaId);
        var alunoExistente = await _alunoRepository.ObterPorIdAsync(alunoId);

        if (rotaExistente is null)
        {
            throw new InvalidOperationException("A rota especificado não existe.");
        }

        if (alunoExistente == null)
        {
            throw new InvalidOperationException("O aluno especificado não existe.");
        }

        if (alunoRota is null)
        {
            var AlunoRota = new AlunoRota
            {
                AlunoId = alunoId,
                RotaId = rotaId
            };

            await _alunoRotaRepository.AdicionarAsync(AlunoRota);
        }
        else
        {
            alunoRota.Status = StatusEntityEnum.Ativo;
            await _alunoRotaRepository.AtualizarAsync(alunoRota);
        }
    }

    public async Task DesvincularRotaAsync(int rotaId, int alunoId)
    {
        if (rotaId < 1 || alunoId < 1)
            return;

        var alunoRota = await _alunoRotaRepository.BuscarUmAsync(x =>
            x.AlunoId == alunoId &&
            x.RotaId == rotaId);

        if (alunoRota is null)
        {
            throw new Exception("Nenhuma rota encontrada!");
        }

        alunoRota.Status = StatusEntityEnum.Deletado;
        await _alunoRotaRepository.AtualizarAsync(alunoRota);
    }

    public async Task<IList<AlunoViewModel>> ObterAlunosAsync(List<int> alunosIds)
    {
        var alunos = await _alunoRepository.BuscarAsync(x =>
            alunosIds.Contains(x.Id),
            z => z.EnderecoPartida,
            z => z.EnderecoDestino,
            z => z.EnderecoRetorno
        );

        return _mapper.Map<List<AlunoViewModel>>(alunos);
    }

    public async Task<List<AlunoRotaViewModel>> ObterRotasPorAlunoAsync(int alunoId, int rotaId)
    {
        var alunosRotas = await _alunoRotaRepository.BuscarAsync(x => x.AlunoId == alunoId && x.RotaId == rotaId);
        return _mapper.Map<List<AlunoRotaViewModel>>(alunosRotas);
    }
}