using System;
using System.Linq;
using Pessoas.Core.Data.APIs;
using Pessoas.Core.Data.Implementations;
using Pessoas.Core.Data.Repositories;
using Pessoas.Core.Domain.Interfaces.APIs;
using Pessoas.Core.Domain.Interfaces.Repositories;
using Pessoas.Core.Domain.Interfaces.Repository;
using Pessoas.Core.Domain.Models;
using Pessoas.Core.Application.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Pessoas.Core.API.Extensions;

public static class RepositoryExtensions
{
    public static IServiceCollection AddCustomRepository(
        this IServiceCollection services,
        SecretManager secretManager)
    {
        services.AddScoped<IUserContext, UserContext>();

        // APIs
        services.AddScoped<IRouterAPI, RouterAPI>();

        // Repositories
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IBaseRepository<AjusteAlunoRota>, BaseRepository<AjusteAlunoRota>>();
        services.AddScoped<IBaseRepository<RotaHistorico>, BaseRepository<RotaHistorico>>();
        services.AddScoped<IBaseRepository<Aluno>, BaseRepository<Aluno>>();
        services.AddScoped<IRedisRepository, RedisRepository>();

        services.AddQueue(secretManager);

        Console.WriteLine("Configuração de repository realizada com sucesso!");

        return services;
    }

    public static IServiceCollection AddQueue(this IServiceCollection services, SecretManager secretManager)
    {
        var connection = secretManager.ConnectionStrings.RabbitConnection.Split(':');

        services.AddSingleton(sp =>
            new ConnectionFactory
            {
                HostName = connection.ElementAt(0), //"localhost",
                Port = int.Parse(connection.ElementAt(1)), // 5672,
                UserName = connection.ElementAt(2), // admin
                Password = connection.ElementAt(3) // admin
            }
        );

        services.AddScoped<IRabbitMqRepository, RabbitMqRepository>();

        return services;
    }
}