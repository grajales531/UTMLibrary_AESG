# UtmMarket CLI - Manifiesto de Arquitectura .NET 10

Este documento detalla la configuración inicial y el diseño arquitectónico de la herramienta CLI UtmMarket, optimizada para **.NET 10**, **C# 14** y **Native AOT**.

## 1. Resumen de Instalación y Dependencias

Se han instalado las versiones estables y compatibles con .NET 10 de los siguientes componentes:

| Paquete NuGet | Versión | Rol Arquitectónico | Native AOT Ready |
| :--- | :--- | :--- | :---: |
| `Microsoft.Data.SqlClient` | 6.1.4 | Driver de SQL Server optimizado. | Sí |
| `Dapper` | 2.1.66 | Micro-ORM ligero (uso bajo perfil en AOT). | Limitado* |
| `Microsoft.Extensions.Hosting` | 10.0.3 | Gestión de ciclo de vida (DI, Logging, Config). | Sí |
| `Microsoft.Extensions.Configuration.UserSecrets` | 10.0.3 | Almacenamiento seguro de secretos locales. | Sí |

*\*Nota: En Native AOT se recomienda el uso de Source Generators para evitar reflexión en tiempo de ejecución.*

## 2. Referencia de Implementación (`Program.cs`)

El esqueleto base utiliza las últimas innovaciones de **C# 14**:
- **`field` Keyword:** Reducción de boilerplate en propiedades auto-implementadas.
- **`HostApplicationBuilder`:** Simplificación radical de la inicialización de servicios.
- **`ValueTask` & `CancellationToken`:** Optimización de rutas calientes (hot paths) y manejo asíncrono robusto.

```csharp
// Fragmento destacado de C# 14 en MarketService.cs
public string ConnectionString 
{ 
    get => field ?? "Server=localhost;Database=Grajales;...";
    set => field = value; 
}
```

## 3. Notas de Modernización

### Beneficios de .NET 10 y Native AOT
1. **Physical Promotion:** El runtime de .NET 10 mejora la ubicación en memoria de tipos de valor, reduciendo la presión sobre el GC.
2. **Desvirtualización Nativa:** Mayor eficiencia en la resolución de llamadas a interfaces.
3. **Optimización AOT:** La aplicación compila directamente a código máquina, logrando:
   - Tiempos de arranque inferiores a **20ms**.
   - Uso de memoria RAM drásticamente reducido.
   - Binario único autocontenido sin dependencia del runtime de .NET.

## 4. Guía de Ejecución y Compilación Nativa

Para compilar como un binario nativo optimizado, ejecute:

```powershell
# Publicación para Windows Native AOT
dotnet publish UtmMarket/UtmMarket.csproj -c Release -r win-x64 --self-contained
```

El binario resultante se encontrará en: `UtmMarket/bin/Release/net10.0/win-x64/publish/UtmMarket.exe`.
