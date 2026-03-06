namespace UtmMarket.Infrastructure.Persistence;

using System.Data;
using Microsoft.Data.SqlClient;

/// <summary>
/// Contrato para la creación de conexiones a la base de datos.
/// </summary>
public interface IDbConnectionFactory
{
    /// <summary>
    /// Crea y abre una nueva conexión asíncronamente.
    /// </summary>
    Task<IDbConnection> CreateConnectionAsync(CancellationToken ct = default);
}

/// <summary>
/// Factoría de conexiones SQL Server optimizada para Native AOT y C# 14.
/// </summary>
public sealed class SqlConnectionFactory(string connectionString) : IDbConnectionFactory
{
    /// <summary>
    /// Cadena de conexión con validación integrada mediante C# 14 field keyword.
    /// </summary>
    public string ConnectionString
    {
        get => field;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) 
                throw new ArgumentException("La cadena de conexión no puede estar vacía.");
            field = value;
        }
    } = connectionString;

    /// <summary>
    /// Crea una conexión abierta hacia SQL Server.
    /// </summary>
    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken ct = default)
    {
        var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync(ct);
        return connection;
    }
}
