namespace MortgageManager.Entities.Models
{
    public class Products(IEnumerable<Product> products)
    {
        private List<Product> _products = products.ToList();

        public IEnumerable<Product> GetAll()
        {
            return _products;
        }

        public void DeleteProduct(string productCode)
        {
            _products.Remove(_products.FirstOrDefault(x => x.ProductCode == productCode));
        }
    }
}
