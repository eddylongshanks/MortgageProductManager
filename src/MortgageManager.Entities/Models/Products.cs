namespace MortgageManager.Entities.Models
{
    public class Products(IEnumerable<Product> products)
    {
        private IEnumerable<Product> _products = products;

        public IEnumerable<Product> GetAll()
        {
            return _products;
        }
    }
}
