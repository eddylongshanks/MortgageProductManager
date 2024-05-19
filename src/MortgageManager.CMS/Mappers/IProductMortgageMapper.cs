
using MortgageManager.CMS.Models;
using MortgageManager.Entities.Models;

namespace MortgageManager.CMS.Mappers
{
    internal interface IProductMortgageMapper
    {
        ProductMortgage Map(Product product);
    }
}
