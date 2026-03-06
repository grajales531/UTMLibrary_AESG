namespace UtmMarket.UI;

using System;

/// <summary>
/// Helper class for standardized console interaction and input validation.
/// </summary>
public static class ConsoleHelper
{
    public static void ShowHeader()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("""
        =========================================================
           UTM MARKET - PRODUCT MANAGEMENT SYSTEM (.NET 10)
        =========================================================
        """);
        Console.ResetColor();
    }

    public static string ReadString(string prompt)
    {
        while (true)
        {
            Console.Write($"{prompt}: ");
            var input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input)) return input.Trim();
            Console.WriteLine("Error: Input cannot be empty.");
        }
    }

    public static int ReadInt(string prompt, int min = 0)
    {
        while (true)
        {
            Console.Write($"{prompt}: ");
            if (int.TryParse(Console.ReadLine(), out int value) && value >= min) return value;
            Console.WriteLine($"Error: Please enter a valid integer (min: {min}).");
        }
    }

    public static decimal ReadDecimal(string prompt, decimal min = 0)
    {
        while (true)
        {
            Console.Write($"{prompt}: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal value) && value >= min) return value;
            Console.WriteLine($"Error: Please enter a valid decimal number (min: {min}).");
        }
    }

    public static void Pause()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
}
