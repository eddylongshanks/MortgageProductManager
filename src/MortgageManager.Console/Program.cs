using MortgageManager.Domain.Helpers;
using MortgageManager.Entities.Models;

namespace MortgageManager
{
    class Program
    {
        static void Main(string[] args)
        {
            var csvManager = new CsvManager("_csv/users.csv");

            Products products = csvManager.ImportUsers();

            foreach (var product in products.GetAll())
            {
                Console.WriteLine($"ID: {product.ProductCode}, Name: {product.Name}");
            }
        }
    }
}
