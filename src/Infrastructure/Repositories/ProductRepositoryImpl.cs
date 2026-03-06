namespace UtmMarket.Infrastructure.Repositories;

using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Models;
using UtmMarket.Core.Repositories;
using UtmMarket.Infrastructure.Mappers;
using UtmMarket.Infrastructure.Models.Data;
using UtmMarket.Infrastructure.Persistence;

/// <summary>
/// Implementación concreta del repositorio de productos optimizada para Native AOT.
/// Utiliza SqlCommand y SqlDataReader para evitar la reflexión dinámica.
/// </summary>
public sealed class ProductRepositoryImpl(IDbConnectionFactory connectionFactory) : IProductRepository
{
    private readonly IDbConnectionFactory _connectionFactory = connectionFactory;

    public async IAsyncEnumerable<Product> GetAllAsync([EnumeratorCancellation] CancellationToken ct = default)
    {
        using var connection = (SqlConnection)await _connectionFactory.CreateConnectionAsync(ct);
        const string sql = "SELECT ProductoID, Nombre, SKU, Marca, Precio, Stock FROM Producto";
        
        using var command = new SqlCommand(sql, connection);
        using var reader = await command.ExecuteReaderAsync(ct);

        while (await reader.ReadAsync(ct))
        {
            yield return MapFromReader(reader).ToDomain();
        }
    }

    public async Task<Product?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        using var connection = (SqlConnection)await _connectionFactory.CreateConnectionAsync(ct);
        const string sql = "SELECT ProductoID, Nombre, SKU, Marca, Precio, Stock FROM Producto WHERE ProductoID = @id";
        
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);
        
        using var reader = await command.ExecuteReaderAsync(ct);
        if (await reader.ReadAsync(ct))
        {
            return MapFromReader(reader).ToDomain();
        }

        return null;
    }

    public async IAsyncEnumerable<Product> FindAsync(ProductFilter filter, [EnumeratorCancellation] CancellationToken ct = default)
    {
        using var connection = (SqlConnection)await _connectionFactory.CreateConnectionAsync(ct);
        var sql = "SELECT ProductoID, Nombre, SKU, Marca, Precio, Stock FROM Producto WHERE 1=1";
        var command = new SqlCommand(string.Empty, connection);

        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            sql += " AND Nombre LIKE @name";
            command.Parameters.AddWithValue("@name", $"%{filter.Name}%");
        }
        if (!string.IsNullOrWhiteSpace(filter.SKU))
        {
            sql += " AND SKU = @sku";
            command.Parameters.AddWithValue("@sku", filter.SKU);
        }
        if (!string.IsNullOrWhiteSpace(filter.Brand))
        {
            sql += " AND Marca LIKE @brand";
            command.Parameters.AddWithValue("@brand", $"%{filter.Brand}%");
        }
        if (filter.MinPrice.HasValue)
        {
            sql += " AND Precio >= @minPrice";
            command.Parameters.AddWithValue("@minPrice", filter.MinPrice.Value);
        }
        if (filter.MaxPrice.HasValue)
        {
            sql += " AND Precio <= @maxPrice";
            command.Parameters.AddWithValue("@maxPrice", filter.MaxPrice.Value);
        }

        command.CommandText = sql;
        using (command)
        {
            using var reader = await command.ExecuteReaderAsync(ct);
            while (await reader.ReadAsync(ct))
            {
                yield return MapFromReader(reader).ToDomain();
            }
        }
    }

    public async Task AddAsync(Product product, CancellationToken ct = default)
    {
        using var connection = (SqlConnection)await _connectionFactory.CreateConnectionAsync(ct);
        const string sql = @"
            INSERT INTO Producto (Nombre, SKU, Marca, Precio, Stock) 
            VALUES (@Nombre, @SKU, @Marca, @Precio, @Stock);
            SELECT CAST(SCOPE_IDENTITY() as int);";

        var entity = product.ToEntity();
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Nombre", entity.Nombre);
        command.Parameters.AddWithValue("@SKU", entity.SKU);
        command.Parameters.AddWithValue("@Marca", entity.Marca ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@Precio", entity.Precio);
        command.Parameters.AddWithValue("@Stock", entity.Stock);

        var id = (int)await command.ExecuteScalarAsync(ct);
        // Nota: En un entorno productivo, podrías querer actualizar el ProductID del objeto de dominio si es necesario.
    }

    public async Task UpdateAsync(Product product, CancellationToken ct = default)
    {
        using var connection = (SqlConnection)await _connectionFactory.CreateConnectionAsync(ct);
        const string sql = @"
            UPDATE Producto 
            SET Nombre = @Nombre, SKU = @SKU, Marca = @Marca, Precio = @Precio, Stock = @Stock 
            WHERE ProductoID = @ProductoID";

        var entity = product.ToEntity();
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@ProductoID", entity.ProductoID);
        command.Parameters.AddWithValue("@Nombre", entity.Nombre);
        command.Parameters.AddWithValue("@SKU", entity.SKU);
        command.Parameters.AddWithValue("@Marca", entity.Marca ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@Precio", entity.Precio);
        command.Parameters.AddWithValue("@Stock", entity.Stock);

        await command.ExecuteNonQueryAsync(ct);
    }

    public async Task UpdateStockAsync(int id, int newStock, CancellationToken ct = default)
    {
        using var connection = (SqlConnection)await _connectionFactory.CreateConnectionAsync(ct);
        const string sql = "UPDATE Producto SET Stock = @Stock WHERE ProductoID = @id";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@Stock", newStock);

        await command.ExecuteNonQueryAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        using var connection = (SqlConnection)await _connectionFactory.CreateConnectionAsync(ct);
        const string sql = "DELETE FROM Producto WHERE ProductoID = @id";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);

        await command.ExecuteNonQueryAsync(ct);
    }

    /// <summary>
    /// Mapeo manual ultra-eficiente para Native AOT (Zero-Reflection).
    /// </summary>
    private static ProductoEntity MapFromReader(SqlDataReader reader)
    {
        return new ProductoEntity(
            productoId: reader.GetInt32(0),
            sku: reader.GetString(2)
        )
        {
            Nombre = reader.GetString(1),
            Marca = reader.IsDBNull(3) ? null : reader.GetString(3),
            Precio = reader.GetDecimal(4),
            Stock = reader.GetInt32(5)
        };
    }
}
