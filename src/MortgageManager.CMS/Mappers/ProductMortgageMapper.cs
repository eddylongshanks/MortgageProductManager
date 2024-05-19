using MortgageManager.CMS.Models;
using MortgageManager.Entities.Models;

namespace MortgageManager.CMS.Mappers;

public class ProductMortgageMapper : IProductMortgageMapper
{
    public ProductMortgage Map(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        var productMortgage = new ProductMortgage()
        {
            Name = product.Name,
            ProductCode = product.ProductCode,
            ComparisonCost = product.ComparisonCost,
        };

        return productMortgage;
    }
}