using System;
using Aluno.Core.Domain.Interfaces.APIs;
using Aluno.Core.Service.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Aluno.Core.API.Extensions;

public static class HttpExtensions
{
    public static IServiceCollection AddCustomHttp(this IServiceCollection services, SecretManager secretManager)
    {
        var url = secretManager.Asaas.Url;
        var asaasToken = secretManager.Asaas.AcessToken;

        services.AddHttpClient("api-router", client =>
        {
            client.BaseAddress = new Uri(secretManager.URL.RouterAPI);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        services.AddRefitClient<IAuthApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(secretManager.URL.AuthAPI));

        Console.WriteLine("Configuração das APIs consumidas realizada com sucesso!");

        return services;
    }
}
