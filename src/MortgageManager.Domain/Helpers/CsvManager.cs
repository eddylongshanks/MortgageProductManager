using CsvHelper;
using MortgageManager.Entities.Models;
using System.Globalization;

namespace MortgageManager.Domain.Helpers
{
    public class CsvManager(string filepath)
    {
        public Products ImportUsers()
        {
            try
            {
                var listOfProducts = new List<Product>();

                using (var reader = new StreamReader(filepath))
                using (CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    IEnumerable<Product> products = csvReader.GetRecords<Product>();
                    
                    foreach (var p in products)
                    {
                        listOfProducts.Add(p);
                    }
                }

                return new Products(listOfProducts);
            }
            catch (CsvHelper.MissingFieldException ex)
            {
                Console.WriteLine($"Error reading the CSV file. The following record had a missing entry: {Environment.NewLine}" +
                    $"{ex.Context.Parser.RawRecord}" +
                    $"No Products were created.");
                return new Products(new List<Product>());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an unexpected error: {ex.Message}" +
                    $"No Products were created.");
                return new Products(new List<Product>());
            }
        }
    }
}
