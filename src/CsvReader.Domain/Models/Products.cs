namespace CsvReaderApp.Domain.Models
{
    public class Products(IEnumerable<Product>? products = null)
    {
        private IEnumerable<Product> _products = products ??= [];

        public IEnumerable<Product> GetAll()
        {
            return _products;
        }
    }
}
