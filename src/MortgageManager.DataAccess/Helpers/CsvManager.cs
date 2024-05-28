using CsvHelper;
using MortgageManager.DataAccess.Mappers;
using MortgageManager.Entities.Models;
using System.Globalization;

namespace MortgageManager.DataAccess.Helpers
{
    public class CsvManager
    {
        private readonly string _filePath;

        public CsvManager() => _filePath = "_csv/users.csv";
        public CsvManager(string filepath) => _filePath = filepath;

        public Products ImportUsers()
        {
            try
            {
                var listOfProducts = new List<Product>();

                using (var reader = new StreamReader(_filePath))
                using (CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csvReader.Context.RegisterClassMap<CsvMortgageMap>();
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
