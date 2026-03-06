namespace UtmMarket.Infrastructure.Repositories;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
/// Implementación concreta del repositorio de ventas optimizada para Native AOT.
/// Utiliza ADO.NET puro (SqlCommand, SqlDataReader) y mapeo manual profundo.
/// </summary>
public sealed class SaleRepositoryImpl(IDbConnectionFactory connectionFactory, IProductRepository productRepository) : ISaleRepository
{
    private readonly IDbConnectionFactory _connectionFactory = connectionFactory;
    private readonly IProductRepository _productRepository = productRepository;

    public async IAsyncEnumerable<Sale> GetAllAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using var connection = (SqlConnection)await _connectionFactory.CreateConnectionAsync(cancellationToken);
        
        // Recuperar todas las ventas
        const string sqlVentas = "SELECT VentaID, Folio, FechaVenta, TotalArticulos, TotalVenta, Estatus FROM Venta";
        var ventasEntities = new List<VentaEntity>();
        
        using (var cmdVentas = new SqlCommand(sqlVentas, connection))
        using (var reader = await cmdVentas.ExecuteReaderAsync(cancellationToken))
        {
            while (await reader.ReadAsync(cancellationToken))
            {
                ventasEntities.Add(MapVentaFromReader(reader));
            }
        }

        // Para cada venta, cargar sus detalles y productos (Optimización: Cargar productos necesarios en lote)
        // Nota: En una implementación de alto volumen, esto se haría con JOINs o QueryMultiple.
        // Aquí seguimos la lógica de reconstrucción de dominio limpia.
        foreach (var vEntity in ventasEntities)
        {
            yield return await GetByIdAsync(vEntity.VentaID, cancellationToken) 
                         ?? throw new InvalidOperationException($"Error al cargar la venta {vEntity.Folio}");
        }
    }

    public async Task<Sale?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        using var connection = (SqlConnection)await _connectionFactory.CreateConnectionAsync(cancellationToken);
        
        // 1. Obtener Cabecera
        VentaEntity? vEntity = null;
        const string sqlVenta = "SELECT VentaID, Folio, FechaVenta, TotalArticulos, TotalVenta, Estatus FROM Venta WHERE VentaID = @id";
        using (var cmd = new SqlCommand(sqlVenta, connection))
        {
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
            if (await reader.ReadAsync(cancellationToken))
            {
                vEntity = MapVentaFromReader(reader);
            }
        }

        if (vEntity == null) return null;

        // 2. Obtener Detalles
        var dEntities = new List<DetalleVentaEntity>();
        const string sqlDetalles = "SELECT DetalleID, VentaID, ProductoID, PrecioUnitario, Cantidad, TotalDetalle FROM DetalleVenta WHERE VentaID = @id";
        using (var cmd = new SqlCommand(sqlDetalles, connection))
        {
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                dEntities.Add(MapDetalleFromReader(reader));
            }
        }

        // 3. Obtener Productos asociados para reconstruir el dominio
        var productIds = dEntities.Select(d => d.ProductoID).Distinct();
        var productsMap = new Dictionary<int, Product>();
        foreach (var pId in productIds)
        {
            var product = await _productRepository.GetByIdAsync(pId, cancellationToken);
            if (product != null) productsMap[pId] = product;
        }

        // 4. Mapear al Dominio usando SaleMapper
        return vEntity.ToDomain(dEntities, productsMap);
    }

    public async IAsyncEnumerable<Sale> FindAsync(SaleFilter filter, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using var connection = (SqlConnection)await _connectionFactory.CreateConnectionAsync(cancellationToken);
        
        var sql = "SELECT VentaID FROM Venta WHERE 1=1";
        var cmd = new SqlCommand(string.Empty, connection);

        if (!string.IsNullOrWhiteSpace(filter.Folio))
        {
            sql += " AND Folio = @folio";
            cmd.Parameters.AddWithValue("@folio", filter.Folio);
        }
        if (filter.Status.HasValue)
        {
            sql += " AND Estatus = @status";
            cmd.Parameters.AddWithValue("@status", (byte)filter.Status.Value);
        }
        if (filter.StartDate.HasValue)
        {
            sql += " AND FechaVenta >= @start";
            cmd.Parameters.AddWithValue("@start", filter.StartDate.Value);
        }
        if (filter.EndDate.HasValue)
        {
            sql += " AND FechaVenta <= @end";
            cmd.Parameters.AddWithValue("@end", filter.EndDate.Value);
        }

        cmd.CommandText = sql;
        var ids = new List<int>();
        using (cmd)
        {
            using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                ids.Add(reader.GetInt32(0));
            }
        }

        foreach (var id in ids)
        {
            var sale = await GetByIdAsync(id, cancellationToken);
            if (sale != null) yield return sale;
        }
    }

    public async Task<Sale> AddAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        using var connection = (SqlConnection)await _connectionFactory.CreateConnectionAsync(cancellationToken);
        // Iniciamos transacción para asegurar integridad del agregado
        using var transaction = connection.BeginTransaction();

        try
        {
            // 1. Insertar Venta
            const string sqlVenta = @"
                INSERT INTO Venta (Folio, FechaVenta, TotalArticulos, TotalVenta, Estatus)
                VALUES (@Folio, @Fecha, @Articulos, @Total, @Estatus);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            var vEntity = sale.ToEntity();
            int ventaId;
            using (var cmd = new SqlCommand(sqlVenta, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@Folio", vEntity.Folio);
                cmd.Parameters.AddWithValue("@Fecha", vEntity.FechaVenta);
                cmd.Parameters.AddWithValue("@Articulos", vEntity.TotalArticulos);
                cmd.Parameters.AddWithValue("@Total", vEntity.TotalVenta);
                cmd.Parameters.AddWithValue("@Estatus", vEntity.Estatus);
                ventaId = (int)await cmd.ExecuteScalarAsync(cancellationToken);
            }

            // 2. Insertar Detalles
            const string sqlDetalle = @"
                INSERT INTO DetalleVenta (VentaID, ProductoID, PrecioUnitario, Cantidad, TotalDetalle)
                VALUES (@VentaID, @ProductoID, @Precio, @Cantidad, @Total);";

            foreach (var dEntity in sale.ToDetailEntities())
            {
                using var cmd = new SqlCommand(sqlDetalle, connection, transaction);
                cmd.Parameters.AddWithValue("@VentaID", ventaId);
                cmd.Parameters.AddWithValue("@ProductoID", dEntity.ProductoID);
                cmd.Parameters.AddWithValue("@Precio", dEntity.PrecioUnitario);
                cmd.Parameters.AddWithValue("@Cantidad", dEntity.Cantidad);
                cmd.Parameters.AddWithValue("@Total", dEntity.TotalDetalle);
                await cmd.ExecuteNonQueryAsync(cancellationToken);
            }

            transaction.Commit();
            return await GetByIdAsync(ventaId, cancellationToken) ?? throw new InvalidOperationException("Error al recuperar la venta recién creada.");
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        using var connection = (SqlConnection)await _connectionFactory.CreateConnectionAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        try
        {
            // 1. Actualizar Cabecera
            const string sqlUpdateVenta = @"
                UPDATE Venta SET Estatus = @Estatus, TotalArticulos = @Articulos, TotalVenta = @Total 
                WHERE VentaID = @VentaID";
            
            var vEntity = sale.ToEntity();
            using (var cmd = new SqlCommand(sqlUpdateVenta, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@VentaID", vEntity.VentaID);
                cmd.Parameters.AddWithValue("@Estatus", vEntity.Estatus);
                cmd.Parameters.AddWithValue("@Articulos", vEntity.TotalArticulos);
                cmd.Parameters.AddWithValue("@Total", vEntity.TotalVenta);
                await cmd.ExecuteNonQueryAsync(cancellationToken);
            }

            // 2. Reemplazar Detalles (Estrategia simple de eliminar y re-insertar para mantener atomicidad)
            const string sqlDeleteDetails = "DELETE FROM DetalleVenta WHERE VentaID = @VentaID";
            using (var cmd = new SqlCommand(sqlDeleteDetails, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@VentaID", vEntity.VentaID);
                await cmd.ExecuteNonQueryAsync(cancellationToken);
            }

            const string sqlInsertDetalle = @"
                INSERT INTO DetalleVenta (VentaID, ProductoID, PrecioUnitario, Cantidad, TotalDetalle)
                VALUES (@VentaID, @ProductoID, @Precio, @Cantidad, @Total);";

            foreach (var dEntity in sale.ToDetailEntities())
            {
                using var cmd = new SqlCommand(sqlInsertDetalle, connection, transaction);
                cmd.Parameters.AddWithValue("@VentaID", vEntity.VentaID);
                cmd.Parameters.AddWithValue("@ProductoID", dEntity.ProductoID);
                cmd.Parameters.AddWithValue("@Precio", dEntity.PrecioUnitario);
                cmd.Parameters.AddWithValue("@Cantidad", dEntity.Cantidad);
                cmd.Parameters.AddWithValue("@Total", dEntity.TotalDetalle);
                await cmd.ExecuteNonQueryAsync(cancellationToken);
            }

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    private static VentaEntity MapVentaFromReader(SqlDataReader reader)
    {
        return new VentaEntity(reader.GetInt32(0), reader.GetString(1))
        {
            FechaVenta = reader.GetDateTime(2),
            TotalArticulos = reader.GetInt32(3),
            TotalVenta = reader.GetDecimal(4),
            Estatus = reader.GetByte(5)
        };
    }

    private static DetalleVentaEntity MapDetalleFromReader(SqlDataReader reader)
    {
        return new DetalleVentaEntity(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2))
        {
            PrecioUnitario = reader.GetDecimal(3),
            Cantidad = reader.GetInt32(4),
            TotalDetalle = reader.GetDecimal(5)
        };
    }
}
