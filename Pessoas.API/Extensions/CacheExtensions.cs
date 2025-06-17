using System;
using Microsoft.Extensions.DependencyInjection;
using Pessoas.Core.Application.Configuration;
using Pessoas.Data.Implementations;
using Pessoas.Domain.Interfaces.Repositories;
using StackExchange.Redis;

namespace Pessoas.Core.API.Extensions;

public static class CacheExtensions
{
    public static IServiceCollection AddCache(this IServiceCollection services, SecretManager secretManager)
    {
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var configuration = secretManager.Infra.Redis;
            return ConnectionMultiplexer.Connect(configuration);
        });

        services.AddScoped<IRedisRepository, RedisRepository>();
        Console.WriteLine("Configuração do Redis realizada com sucesso!");

        return services;
    }
}