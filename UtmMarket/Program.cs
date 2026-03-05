using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Dapper;

namespace UtmMarket;

/// <summary>
/// Aplicación CLI UtmMarket optimizada para .NET 10 y Native AOT.
/// </summary>
public sealed class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        // Configuración de Servicios (DI)
        ConfigureServices(builder.Services, builder.Configuration);

        using var host = builder.Build();
        
        // Punto de entrada de la lógica de negocio
        await RunAsync(host.Services);
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // Registro de dependencias siguiendo Zero Trust y Clean Code
        // Nota: Native AOT requiere evitar reflexión en DI si es posible
        services.AddSingleton<MarketService>();
    }

    private static async Task RunAsync(IServiceProvider services)
    {
        var service = services.GetRequiredService<MarketService>();
        await service.ExecuteAsync(CancellationToken.None);
    }
}

/// <summary>
/// Servicio de Mercado utilizando C# 14 y optimizaciones de Native AOT.
/// </summary>
public sealed class MarketService
{
    // C# 14: Uso de la palabra clave 'field' en propiedades
    public string ConnectionString 
    { 
        get => field ?? "Server=localhost;Database=Grajales;Trusted_Connection=True;TrustServerCertificate=True;";
        set => field = value; 
    }

    /// <summary>
    /// Ejecuta la lógica principal con soporte para CancellationToken y ValueTask.
    /// </summary>
    public async ValueTask ExecuteAsync(CancellationToken ct)
    {
        Console.WriteLine("--- UtmMarket CLI .NET 10 ---");
        Console.WriteLine($"Status: Ready (Native AOT Optimizations Active)");

        try 
        {
            // Ejemplo de acceso a datos AOT-safe (evitando reflexión pesada de Dapper si es necesario)
            // En .NET 10, priorizamos source generators para Dapper o ADO.NET puro.
            await using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync(ct);
            
            Console.WriteLine("Conexión a base de datos [Grajales] establecida.");
            
            // Simulación de consulta rápida (Fast Path)
            var version = await connection.ExecuteScalarAsync<string>("SELECT @@VERSION");
            Console.WriteLine($"Motor: {version?.Split('\n')[0]}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error crítico: {ex.Message}");
        }
    }
}
