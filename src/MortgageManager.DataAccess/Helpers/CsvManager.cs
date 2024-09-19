using CsvHelper;
using Microsoft.Extensions.Logging;
using MortgageManager.DataAccess.Mappers;
using MortgageManager.Entities.Models;
using System.Globalization;

namespace MortgageManager.DataAccess.Helpers
{
    public class CsvManager
    {
        private ILogger _logger;

        public string FileName { get; set; } = string.Empty;

        public CsvManager(ILogger<CsvManager> logger)
        {
            _logger = logger;
        }

        public Products ImportProducts()
        {
            try
            {
                var listOfProducts = new List<Product>();

                using (var stream = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = new StreamReader(stream))
                using (CsvReader csvReader = new CsvReader(reader, CultureInfo.GetCultureInfo("en-GB")))
                {
                    csvReader.Context.RegisterClassMap<CsvMortgageMap>();
                    IEnumerable<Product> products = csvReader.GetRecords<Product>();
                    
                    foreach (var p in products)
                    {
                        if (p.ProductCode != null)
                        {
                            listOfProducts.Add(p);
                        }
                    }
                }

                return new Products(listOfProducts);
            }
            catch (CsvHelper.HeaderValidationException hvex)
            {
                _logger.LogError(hvex.Message);
                throw new Exception($"""The expected header "{hvex.InvalidHeaders.FirstOrDefault()?.Names.FirstOrDefault()}" does not exist in the specified file.""", hvex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception($"Data was invalid. Error: {ex.GetType().Name}", ex);
            }
        }
    }
}
