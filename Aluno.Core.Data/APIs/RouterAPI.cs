using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Aluno.Core.Domain.Interfaces.APIs;
using Aluno.Core.Domain.Interfaces.Repository;
using Aluno.Core.Domain.Utils;
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
}