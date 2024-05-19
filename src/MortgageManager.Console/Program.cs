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
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    
                }

                if (productCreated)
                    Console.WriteLine($"Product Created. (ID: {product.ProductCode}, Name: {product.Name})");
                else
                    Console.WriteLine("Product creation failed.");
            }
        }
    }
}
