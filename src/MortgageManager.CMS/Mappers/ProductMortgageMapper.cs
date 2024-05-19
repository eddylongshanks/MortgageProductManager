using MortgageManager.CMS.Models;
using MortgageManager.Entities.Models;

namespace MortgageManager.CMS.Mappers;

public class ProductMortgageMapper : IProductMortgageMapper
{
    public IProductMortgage Map(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        var productMortgage = new ProductMortgage()
        {
            ClientType = MapClientType(product.ClientType),
            ComparisonCost = product.APRC,
            DealTerm = MapDealTerm(product.DealTerm),
            Fees = product.Fees,
            FullDescription = product.FullDescription,
            Heading = product.Heading,
            Illustration = product.Illustration,
            InitialInterestRate = product.InitialInterestRate,
            MaturityDate = product.MaturityDate,
            MaximumLtv = product.MaximumLtv,
            //MortgageTypes = product.MortgageTypes,
            Name = product.Name,
            ProductCode = product.ProductCode,
            RateType = MapRateType(product.RateType),
            StandardVariableRate = product.StandardVariableRate,
        };

        return productMortgage;
    }

    private string MapClientType(IEnumerable<string?> clientType) => clientType switch
    {
        //"new_customer" => "new",
        //"existing_customer" => "existing",
        //_ => throw new ArgumentException($"""The provided value of "{nameof(clientType)}" was invalid"""),

        _ => throw new NotImplementedException()
    };

    private string MapDealTerm(string? dealTerm) => dealTerm switch
    {
        "1" => "one_year",
        "2" => "two_years",
        "3" => "three_years",
        "4" => "four_years",
        "5" => "five_years",
        _ => throw new ArgumentException($"""The provided value of "{nameof(dealTerm)}" was invalid"""),
    };

    private string MapRateType(string? rateType) => rateType switch
    {
        "fixed_rate" => "fixed",
        "discounted_variable_rate" => "variable",
        "base_rate_tracker" => "tracker",
        _ => throw new ArgumentException($"""The provided value of "{nameof(rateType)}" was invalid"""),
    };
}