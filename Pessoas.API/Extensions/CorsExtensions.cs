using System;
using Microsoft.Extensions.DependencyInjection;

namespace Aluno.Core.API.Extensions;

public static class CorsExtensions
{
    public static IServiceCollection AddCustomCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .SetIsOriginAllowed(_ => true) // Permitir todas as origens
                    .AllowCredentials();
            });
        });

        Console.WriteLine("Configuração do CORS realizada com sucesso!");

        return services;
    }
}
