using System;
using Pessoas.Core.Application.Implementations;
using Pessoas.Core.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Pessoas.Core.API.Filters;
using FluentValidation.AspNetCore;
using FluentValidation;
using System.Reflection;
using Pessoas.Core.Application.Configuration;

namespace Pessoas.Core.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services, SecretManager secretManager)
    {
        services.AddHttpContextAccessor();
        services.AddCache(secretManager);

        services.AddScoped<IAlunoService, AlunoService>();
        services.AddScoped<IMotoristaService, MotoristaService>();

        Console.WriteLine("Configuração das services realizada com sucesso!");

        return services;
    }

    public static IServiceCollection AddCache(this IServiceCollection services, SecretManager secretManager)
    {
        Console.WriteLine("Configuração do Redis realizada com sucesso!");

        return services;
    }

    public static IServiceCollection AddCustomMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { }, AppDomain.CurrentDomain.GetAssemblies());
        return services;
    }

    public static IServiceCollection AddControllersWithFilters(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add<GlobalExceptionFilter>();
        });

        services.AddSignalR();

        services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}