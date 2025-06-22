using System;
using Pessoas.Core.Domain.Interfaces.APIs;
using Pessoas.Core.Application.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Pessoas.Core.API.Extensions;

public static class HttpExtensions
{
    public static IServiceCollection AddCustomHttp(this IServiceCollection services, SecretManager secretManager)
    {
        services.AddHttpClient("api-router", client =>
        {
            client.BaseAddress = new Uri(secretManager.URL.RouterAPI);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        services.AddRefitClient<IAuthApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(secretManager.URL.GatewayAPI));

        Console.WriteLine("Configuração das APIs consumidas realizada com sucesso!");

        return services;
    }
}
