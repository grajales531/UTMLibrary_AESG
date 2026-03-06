namespace UtmMarket.Infrastructure;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using UtmMarket.Core.Repositories;
using UtmMarket.Infrastructure.Persistence;
using UtmMarket.Infrastructure.Repositories;

/// <summary>
/// Contenedor de extensiones para el registro de servicios de infraestructura.
/// Utiliza convenciones modernas de C# 14.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registra la infraestructura de persistencia con Dapper AOT y SqlConnectionFactory.
    /// </summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("La cadena de conexión 'DefaultConnection' no fue encontrada.");

        // Registro de la factoría como Singleton para eficiencia en Native AOT
        services.AddSingleton<IDbConnectionFactory>(_ => new SqlConnectionFactory(connectionString));
        
        // Registro de Repositorios optimizados para AOT
        services.AddScoped<IProductRepository, ProductRepositoryImpl>();
        services.AddScoped<ISaleRepository, SaleRepositoryImpl>();

        return services;
    }
}
