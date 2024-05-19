using MortgageManager.CMS;
using MortgageManager.Domain.Helpers;
using MortgageManager.Entities.Models;

namespace MortgageManager
{
    class Program
    {
        static void Main(string[] args)
        {
            var csvManager = new CsvManager("_csv/users.csv");
            var mortgageManager = new MortgageCreatorConsole();

            Products products = csvManager.ImportUsers();

            foreach (var product in products.GetAll())
            {
                var productCreated = false;
                try
                {
                    productCreated = mortgageManager.UploadMortgageProduct(product).Result;
                    if (productCreated)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Created Product: {product.ProductCode}");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Failed to create product: {product.ProductCode}");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                
            }
        }
    }
}
