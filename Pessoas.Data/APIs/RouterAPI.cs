using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Aluno.Core.Domain.Interfaces.APIs;
using Aluno.Core.Domain.Interfaces.Repository;
using Aluno.Core.Domain.Utils;
using Aluno.Core.Domain.ViewModels;
using Aluno.Core.Domain.ViewModels.Rota;
using Microsoft.Extensions.Logging;

namespace Aluno.Core.Data.APIs;

public class RouterAPI : IRouterAPI
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<RouterAPI> _logger;
    private readonly IUserContext _context;
    public RouterAPI(IHttpClientFactory httpClientFactory, IUserContext context, ILogger<RouterAPI> logger)
    {
        _context = context;
        _httpClient = httpClientFactory.CreateClient("api-router");
        _logger = logger;
    }

    public async Task<RotaViewModel> ObterRotaPorIdAsync(int rotaId)
    {
        _logger.LogInformation($"Enviando requisição para obter dados da rota - Dados: {rotaId}");
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_context.Token}");
        var response = await _httpClient.GetAsync($"/api/v1/Rota/{rotaId}");

        if (response.IsSuccessStatusCode)
        {
            var rota = await response.Content.ReadFromJsonAsync<RotaViewModel>();
            _logger.LogInformation($"Resposta da requisição para obter dados da rota - Dados: {rota.ToJson()}");
            return rota;
        }
        else
        {
            var mensagemErro = await response.Content.ReadAsStringAsync();
            _logger.LogError($"Erro ao obter dados da rota - Mensagem: {mensagemErro}");
            throw new Exception("Ocorreu um erro ao tentar obter dados da rota!");
        }
    }

    #region Aluno Rota
    public async Task<BaseResponse<List<AlunoRotaViewModel>>> ObterRotasPorAlunoAsync(int rotaId, int? alunoId = null)
    {
        _logger.LogInformation($"Enviando requisição para obter rotas por aluno - Dados: {alunoId}");

        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_context.Token}");

        var response = await _httpClient.GetAsync($"/api/v1/AlunoRota?rotaId={rotaId}&alunoId={alunoId}");

        if (response.IsSuccessStatusCode)
        {
            var alunoRota = await response.Content.ReadFromJsonAsync<BaseResponse<List<AlunoRotaViewModel>>>();
            _logger.LogInformation($"Resposta da requisição para obter rotas por aluno - Dados: {alunoRota.ToJson()}");
            return alunoRota;
        }
        else
        {
            var mensagemErro = await response.Content.ReadAsStringAsync();
            _logger.LogError($"Erro ao obter rotas por aluno - Mensagem: {mensagemErro}");
            throw new Exception("Ocorreu um erro ao tentar obter rotas por aluno!");
        }
    }

    public async Task<BaseResponse<object>> AdicionarAlunoRotaAsync(AlunoRotaViewModel alunoRotaAdicionarViewModel)
    {
        _logger.LogInformation($"Enviando requisição para adicionar aluno a rota - Dados: {alunoRotaAdicionarViewModel.ToJson()}");

        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_context.Token}");

        var response = await _httpClient.PostAsJsonAsync("/api/v1/AlunoRota", alunoRotaAdicionarViewModel);

        if (response.IsSuccessStatusCode)
        {
            var alunoRota = await response.Content.ReadFromJsonAsync<BaseResponse<object>>();
            _logger.LogInformation($"Aluno adicionado a rota com sucesso!");
            return alunoRota;
        }
        else
        {
            var mensagemErro = await response.Content.ReadAsStringAsync();
            _logger.LogError($"Erro ao adicionar aluno a rota - Mensagem: {mensagemErro}");
            throw new Exception("Ocorreu um erro ao tentar adicionar o aluno a rota!");
        }
    }

    public async Task<BaseResponse<object>> AtualizarAlunoRotaAsync(AlunoRotaViewModel alunoRotaAtualizarViewModel)
    {
        _logger.LogInformation($"Enviando requisição para atualizar aluno a rota - Dados: {alunoRotaAtualizarViewModel.ToJson()}");

        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_context.Token}");

        var response = await _httpClient.PutAsJsonAsync($"/api/v1/AlunoRota", alunoRotaAtualizarViewModel);

        if (response.IsSuccessStatusCode)
        {
            var alunoRota = await response.Content.ReadFromJsonAsync<BaseResponse<object>>();
            _logger.LogInformation($"Aluno atualizado na rota com sucesso!");
            return alunoRota;
        }
        else
        {
            var mensagemErro = await response.Content.ReadAsStringAsync();
            _logger.LogError($"Erro ao atualizar aluno a rota - Mensagem: {mensagemErro}");
            throw new Exception("Ocorreu um erro ao tentar atualizar o aluno a rota!");
        }
    }

    #endregion Aluno Rota

    #region Endereco
    public async Task AdicionarEnderecoAsync(EnderecoAdicionarViewModel enderecoAdicionarViewModel)
    {
        _logger.LogInformation($"Enviando requisição para adicionar um novo endereço - Dados: {enderecoAdicionarViewModel.ToJson()}");

        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_context.Token}");

        var response = await _httpClient.PostAsJsonAsync("/api/v1/endereco", enderecoAdicionarViewModel);

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation($"Endereço adicionado com sucesso!");
        }
        else
        {
            var mensagemErro = await response.Content.ReadAsStringAsync();
            _logger.LogError($"Erro ao adicionar endereço - Mensagem: {mensagemErro}");
            throw new Exception("Ocorreu um erro ao tentar adicionar o endereço!");
        }
    }

    public async Task AtualizarEnderecoAsync(int id, EnderecoAtualizarViewModel enderecoAtualizarViewModel)
    {
        _logger.LogInformation($"Enviando requisição para atualizar endereço - ID: {id}, Dados: {enderecoAtualizarViewModel.ToJson()}");

        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_context.Token}");

        var response = await _httpClient.PutAsJsonAsync($"/api/v1/endereco/{id}", enderecoAtualizarViewModel);

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation($"Endereço atualizado com sucesso!");
        }
        else
        {
            var mensagemErro = await response.Content.ReadAsStringAsync();
            _logger.LogError($"Erro ao atualizar endereço - Mensagem: {mensagemErro}");
            throw new Exception("Ocorreu um erro ao tentar atualizar o endereço!");
        }
    }

    public async Task DeletarEnderecoAsync(int id)
    {
        _logger.LogInformation($"Enviando requisição para deletar endereço - ID: {id}");

        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_context.Token}");

        var response = await _httpClient.DeleteAsync($"/api/v1/endereco/{id}");

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation($"Endereço deletado com sucesso!");
        }
        else
        {
            var mensagemErro = await response.Content.ReadAsStringAsync();
            _logger.LogError($"Erro ao deletar endereço - Mensagem: {mensagemErro}");
            throw new Exception("Ocorreu um erro ao tentar deletar o endereço!");
        }
    }

    public async Task<EnderecoViewModel> ObterEnderecoPorIdAsync(int id)
    {
        _logger.LogInformation($"Enviando requisição para obter dados do endereço - ID: {id}");

        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_context.Token}");

        var response = await _httpClient.GetAsync($"/api/v1/endereco/{id}");

        if (response.IsSuccessStatusCode)
        {
            var endereco = await response.Content.ReadFromJsonAsync<EnderecoViewModel>();
            _logger.LogInformation($"Resposta da requisição para obter dados do endereço - Dados: {endereco.ToJson()}");
            return endereco;
        }
        else
        {
            var mensagemErro = await response.Content.ReadAsStringAsync();
            _logger.LogError($"Erro ao obter dados do endereço - Mensagem: {mensagemErro}");
            throw new Exception("Ocorreu um erro ao tentar obter dados do endereço!");
        }
    }

    public async Task<List<EnderecoViewModel>> ObterEnderecoPorIdAsync(List<int> ids)
    {
        _logger.LogInformation($"Enviando requisição para obter dados do endereço - ID: {ids.ToJson()}");

        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_context.Token}");

        var response = await _httpClient.GetAsync($"/api/v1/endereco/{ids}");

        if (response.IsSuccessStatusCode)
        {
            var enderecos = await response.Content.ReadFromJsonAsync<List<EnderecoViewModel>>();
            _logger.LogInformation($"Resposta da requisição para obter dados do endereço - Dados: {enderecos.ToJson()}");
            return enderecos;
        }
        else
        {
            var mensagemErro = await response.Content.ReadAsStringAsync();
            _logger.LogError($"Erro ao obter dados do endereço - Mensagem: {mensagemErro}");
            throw new Exception("Ocorreu um erro ao tentar obter dados do endereço!");
        }
    }

    public async Task<IEnumerable<EnderecoViewModel>> ObterTodosEnderecosAsync()
    {
        _logger.LogInformation($"Enviando requisição para obter todos os endereços");

        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_context.Token}");

        var response = await _httpClient.GetAsync("/api/v1/endereco");

        if (response.IsSuccessStatusCode)
        {
            var enderecos = await response.Content.ReadFromJsonAsync<IEnumerable<EnderecoViewModel>>();
            _logger.LogInformation($"Resposta da requisição para obter todos os endereços - Dados: {enderecos.ToJson()}");
            return enderecos;
        }
        else
        {
            var mensagemErro = await response.Content.ReadAsStringAsync();
            _logger.LogError($"Erro ao obter todos os endereços - Mensagem: {mensagemErro}");
            throw new Exception("Ocorreu um erro ao tentar obter todos os endereços!");
        }
    }
    #endregion
}