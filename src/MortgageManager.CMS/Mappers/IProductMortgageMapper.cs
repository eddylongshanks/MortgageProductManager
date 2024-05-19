
using MortgageManager.CMS.Models;
using MortgageManager.Entities.Models;

namespace MortgageManager.CMS.Mappers
{
    public interface IProductMortgageMapper
    {
        IProductMortgage Map(Product product);
    }
}
