namespace UtmMarket;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using UtmMarket.Infrastructure;
using UtmMarket.Application;
using UtmMarket.Core.UseCases;
using UtmMarket.Core.Entities;
using UtmMarket.UI;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Entry point for UtmMarket CLI .NET 10.
/// Manages the host lifecycle and the main interactive loop.
/// </summary>
public sealed class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        // Ensure user secrets are loaded in Development
        if (builder.Environment.IsDevelopment())
        {
            builder.Configuration.AddUserSecrets<Program>();
        }

        // Register Layers
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddApplication();

        using var host = builder.Build();
        
        await RunMainLoopAsync(host.Services);
    }

    private static async Task RunMainLoopAsync(IServiceProvider services)
    {
        bool exit = false;
        while (!exit)
        {
            ConsoleHelper.ShowHeader();
            Console.WriteLine("""
            1. List all products
            2. Find product by ID
            3. Register new product
            4. Exit
            """);
            
            int choice = ConsoleHelper.ReadInt("\nSelect an option", 1);
            
            // Resolve use cases within a manual scope for better resource management
            using (var scope = services.CreateScope())
            {
                try 
                {
                    switch (choice)
                    {
                        case 1: await HandleListProducts(scope.ServiceProvider); break;
                        case 2: await HandleFindProduct(scope.ServiceProvider); break;
                        case 3: await HandleRegisterProduct(scope.ServiceProvider); break;
                        case 4: exit = true; break;
                        default: Console.WriteLine("Invalid option."); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n[Error]: {ex.Message}");
                    Console.ResetColor();
                }
            }

            if (!exit) ConsoleHelper.Pause();
        }
    }

    private static async Task HandleListProducts(IServiceProvider sp)
    {
        var useCase = sp.GetRequiredService<IGetAllProductsUseCase>();
        Console.WriteLine("\n--- Product List ---");
        Console.WriteLine($"{"ID",-5} | {"SKU",-15} | {"Name",-30} | {"Price",-10} | {"Stock",-5}");
        Console.WriteLine(new string('-', 75));

        await foreach (var p in useCase.ExecuteAsync())
        {
            Console.WriteLine($"{p.ProductID,-5} | {p.SKU,-15} | {p.Name,-30} | {p.Price,10:C} | {p.Stock,5}");
        }
    }

    private static async Task HandleFindProduct(IServiceProvider sp)
    {
        var useCase = sp.GetRequiredService<IGetProductByIdUseCase>();
        int id = ConsoleHelper.ReadInt("Enter Product ID");
        
        var product = await useCase.ExecuteAsync(id);
        if (product == null)
        {
            Console.WriteLine($"Product with ID {id} not found.");
            return;
        }

        Console.WriteLine("\n--- Product Details ---");
        Console.WriteLine($"ID: {product.ProductID}");
        Console.WriteLine($"SKU: {product.SKU}");
        Console.WriteLine($"Name: {product.Name}");
        Console.WriteLine($"Brand: {product.Brand ?? "N/A"}");
        Console.WriteLine($"Price: {product.Price:C}");
        Console.WriteLine($"Stock: {product.Stock}");
    }

    private static async Task HandleRegisterProduct(IServiceProvider sp)
    {
        var useCase = sp.GetRequiredService<ICreateProductUseCase>();
        
        Console.WriteLine("\n--- Register New Product ---");
        string name = ConsoleHelper.ReadString("Name");
        string sku = ConsoleHelper.ReadString("SKU");
        string brand = ConsoleHelper.ReadString("Brand (optional)");
        decimal price = ConsoleHelper.ReadDecimal("Price");
        int stock = ConsoleHelper.ReadInt("Initial Stock");

        var newProduct = new Product(0, name, sku, brand)
        {
            Price = price,
            Stock = stock
        };

        await useCase.ExecuteAsync(newProduct);
        Console.WriteLine("\nProduct registered successfully!");
    }
}
