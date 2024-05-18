using CsvHelper;
using MortgageManager.Entities.Models;
using System.Diagnostics;
using System.Globalization;

namespace MortgageManager.Domain.Helpers
{
    public class CsvManager(string filepath)
    {
        private readonly CsvReader _csvReader = new CsvReader(new StreamReader(filepath), CultureInfo.InvariantCulture);

        public Products ImportUsers()
        {
            try
            {
                IEnumerable<Product> records = _csvReader.GetRecords<Product>();
                return new Products(records);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error reading the CSV file: " + ex.Message);

                return new Products(new List<Product>());
            }
        }
    }
}
