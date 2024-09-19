using MortgageManager.CMS.Helpers;
using MortgageManager.CMS.Models;
using MortgageManager.Entities.Models;

namespace MortgageManager.CMS.Mappers;

public class ProductMortgageMapper : IProductMortgageMapper
{
    private ValueConverters _valueConverters;

    public ProductMortgageMapper()
    {
        _valueConverters = new ValueConverters();
    }

    public IProductMortgage Map(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        try
        {
            var productMortgage = new ProductMortgage()
            {
                Codename = $"mortgage_{product.ProductCode}_{Guid.NewGuid()}".Substring(0, 50).Replace("-", "_").ToLower(),
                ClientType = MapMultipleArrayValues(product.ClientType, _valueConverters.ConvertClientTypes),
                ComparisonCost = product.APRC,
                DealTerm = MapArrayValuesToSingle(product.DealTerm, _valueConverters.ConvertDealTerms),
                Fees = product.Fees,
                FullDescription = product.FullDescription,
                Heading = product.Heading,
                Illustration = product.Illustration,
                InitialInterestRate = product.InitialInterestRate,
                MaturityDate = product.MaturityDate,
                MaximumLtv = product.MaximumLtv,
                MortgageTypes = MapMultipleArrayValues(product.MortgageTypes, _valueConverters.ConvertMortgageTypes),
                Name = product.Name,
                ProductCode = product.ProductCode,
                RateType = MapArrayValuesToSingle(product.RateType, _valueConverters.ConvertRateTypes),
                StandardVariableRate = product.StandardVariableRate,
            };

            return productMortgage;
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Product: {product.ProductCode}:{Environment.NewLine}{ex.Message}");
        }
    }

    private string[] MapMultipleArrayValues(string[] values, Func<string?, string[]> method)
    {
        List<string> convertedValues = [];

        foreach (var value in values)
            foreach (var item in method(value))
                convertedValues.Add(item);            

        return convertedValues.ToArray();
    }

    private string[] MapArrayValuesToSingle(string[] values, Func<string?, string> method)
    {
        List<string> convertedValues = [];
        
        convertedValues.Add(method(values.FirstOrDefault()));

        return convertedValues.ToArray();
    }
}