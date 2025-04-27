using System;
using System.Linq;
using Aluno.Core.Data.APIs;
using Aluno.Core.Data.Implementations;
using Aluno.Core.Data.Repositories;
using Aluno.Core.Domain.Interfaces.APIs;
using Aluno.Core.Domain.Interfaces.Repositories;
using Aluno.Core.Domain.Interfaces.Repository;
using Aluno.Core.Domain.Models;
using Aluno.Core.Service.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Aluno.Core.API.Extensions;

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
        services.AddScoped<IBaseRepository<Domain.Models.Aluno>, BaseRepository<Domain.Models.Aluno>>();
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