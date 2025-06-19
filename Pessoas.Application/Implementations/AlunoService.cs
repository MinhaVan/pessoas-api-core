using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Pessoas.Core.Domain.Interfaces.Repository;
using Pessoas.Core.Domain.Enums;
using Pessoas.Core.Domain.ViewModels;
using Pessoas.Core.Domain.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Pessoas.Core.Domain.Interfaces.APIs;
using Pessoas.Core.Data.Context;

namespace Pessoas.Core.Application.Implementations;

public class AlunoService : IAlunoService
{
    private readonly IMapper _mapper;
    private readonly IBaseRepository<Domain.Models.Aluno> _alunoRepository;
    private readonly IRouterAPI _routerAPI;
    private readonly IUserContext _userContext;
    public AlunoService(
        IRouterAPI routerAPI,
        IBaseRepository<Domain.Models.Aluno> alunoRepository,
        IUserContext userContext,
        IMapper map)
    {
        _userContext = userContext;
        _mapper = map;
        _routerAPI = routerAPI;
        _alunoRepository = alunoRepository;
    }

    public async Task AdicionarAsync(int responsavelId, AlunoAdicionarViewModel AlunoVm)
    {
        var aluno = _mapper.Map<Domain.Models.Aluno>(AlunoVm);
        aluno.ResponsavelId = responsavelId;
        aluno.Status = StatusEntityEnum.Ativo;

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

    public async Task<IList<AlunoViewModel>> ObterTodosPorResponsavelAsync(int responsavelId, bool obterEnderecos = true)
    {
        var alunos = await _alunoRepository.BuscarAsync(x => x.ResponsavelId == responsavelId && x.Status == StatusEntityEnum.Ativo);
        var response = _mapper.Map<List<AlunoViewModel>>(alunos);

        if (obterEnderecos)
        {
            var enderecos = await ObterEnderecosAsync(alunos);
            await Parallel.ForEachAsync(response, async (aluno, _) =>
            {
                aluno.EnderecoPartida = enderecos.FirstOrDefault(x => x.Id == aluno.EnderecoPartidaId);
                aluno.EnderecoDestino = enderecos.FirstOrDefault(x => x.Id == aluno.EnderecoDestinoId);
                aluno.EnderecoRetorno = enderecos.FirstOrDefault(x => x.Id == aluno.EnderecoRetornoId);
            });
        }

        return response;
    }

    public async Task<List<AlunoViewModel>> ObterAlunosPorFiltro(int rotaId, string filtro)
    {
        filtro = filtro.Trim().ToLower();

        // Buscar todos os alunos que correspondem ao filtro
        var alunos = await _alunoRepository.BuscarAsync(
            x => EF.Functions.ILike(APIContext.Unaccent(x.PrimeiroNome), $"%{filtro}%") ||
                EF.Functions.ILike(APIContext.Unaccent(x.UltimoNome), $"%{filtro}%") ||
                EF.Functions.ILike(APIContext.Unaccent(x.PrimeiroNome + " " + x.UltimoNome), $"%{filtro}%") ||
                EF.Functions.ILike(APIContext.Unaccent(x.Contato), $"%{filtro}%") ||
                EF.Functions.ILike(APIContext.Unaccent(x.Email), $"%{filtro}%") ||
                EF.Functions.ILike(APIContext.Unaccent(x.CPF), $"%{filtro}%") &&
                x.Status == StatusEntityEnum.Ativo
        );

        if (alunos is null || !alunos.Any())
            return new List<AlunoViewModel>();

        var enderecos = await ObterEnderecosAsync(alunos);

        // Buscar IDs dos alunos que já estão cadastrados na rota
        var alunoRotaResponse = await _routerAPI.ObterRotasPorAlunoAsync(rotaId);
        if (alunoRotaResponse is null || alunoRotaResponse.Sucesso == false)
            throw new Exception(alunoRotaResponse.Mensagem);

        var alunosNaRota = alunoRotaResponse.Data;
        var alunosNaRotaIds = alunosNaRota.Select(x => x.AlunoId).ToList();

        // Filtrar alunos que NÃO estão na rota
        var alunosForaDaRota = alunos.Where(x => !alunosNaRotaIds.Contains(x.Id)).ToList();

        var response = _mapper.Map<List<AlunoViewModel>>(alunosForaDaRota);
        await Parallel.ForEachAsync(response, async (aluno, _) =>
        {
            aluno.EnderecoPartida = enderecos.FirstOrDefault(x => x.Id == aluno.EnderecoPartidaId);
            aluno.EnderecoDestino = enderecos.FirstOrDefault(x => x.Id == aluno.EnderecoDestinoId);
            aluno.EnderecoRetorno = enderecos.FirstOrDefault(x => x.Id == aluno.EnderecoRetornoId);
        });

        return response;
    }

    private async Task<List<EnderecoViewModel>> ObterEnderecosAsync(IEnumerable<Domain.Models.Aluno> alunos)
    {
        var enderecosParaBuscar = alunos
            .Select(x => x.EnderecoPartidaId)
            .Concat(alunos.Select(x => x.EnderecoDestinoId))
            .Concat(alunos.Select(x => x.EnderecoRetornoId ?? 0))
            .Where(x => x > 0)
            .Distinct()
            .ToList();

        if (!enderecosParaBuscar.Any())
            return new List<EnderecoViewModel>();

        var enderecos = await _routerAPI.ObterEnderecoPorIdAsync(enderecosParaBuscar);
        return enderecos.ToList();
    }

    public async Task<IList<AlunoViewModel>> ObterAluno(int responsavelId, int AlunoId)
    {
        var Alunos = await _alunoRepository.BuscarAsync(x => x.ResponsavelId == responsavelId && x.Id == AlunoId);
        return _mapper.Map<List<AlunoViewModel>>(Alunos);
    }

    public async Task<IList<AlunoViewModel>> ObterAlunosAsync(List<int> alunosIds)
    {
        var alunos = await _alunoRepository.BuscarAsync(x => alunosIds.Contains(x.Id));
        var enderecos = await ObterEnderecosAsync(alunos);

        var response = _mapper.Map<List<AlunoViewModel>>(alunos);
        await Parallel.ForEachAsync(response, async (aluno, _) =>
        {
            aluno.EnderecoPartida = enderecos.FirstOrDefault(x => x.Id == aluno.EnderecoPartidaId);
            aluno.EnderecoDestino = enderecos.FirstOrDefault(x => x.Id == aluno.EnderecoDestinoId);
            aluno.EnderecoRetorno = enderecos.FirstOrDefault(x => x.Id == aluno.EnderecoRetornoId);
        });

        return response;
    }

    public async Task<List<AlunoViewModel>> ObterTodosAsync()
    {
        var alunos = await _alunoRepository.BuscarAsync(x => x.Status == StatusEntityEnum.Ativo);
        return _mapper.Map<List<AlunoViewModel>>(alunos);
    }
}