using System;
using System.Linq;
using Pessoas.Core.Data.APIs;
using Pessoas.Core.Data.Implementations;
using Pessoas.Core.Data.Repositories;
using Pessoas.Core.Domain.Interfaces.APIs;
using Pessoas.Core.Domain.Interfaces.Repository;
using Pessoas.Core.Domain.Models;
using Pessoas.Core.Application.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

        services.AddQueue(secretManager);

        Console.WriteLine("Configuração de repository realizada com sucesso!");

        return services;
    }

    public static IServiceCollection AddQueue(this IServiceCollection services, SecretManager secretManager)
    {
        var connection = secretManager.ConnectionStrings.RabbitConnection.Split(':');



        return services;
    }
}