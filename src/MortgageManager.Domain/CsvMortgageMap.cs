using CsvHelper.Configuration;
using MortgageManager.Domain.Helpers;
using MortgageManager.Entities.Models;

namespace MortgageManager.CMS.Mappers
{
    internal class CsvMortgageMap : ClassMap<Product>
    {
        public CsvMortgageMap()
        {
            Map(m => m.APRC);
            Map(m => m.ClientType).TypeConverter<StringArrayConverter>();
            Map(m => m.DealTerm).TypeConverter<StringArrayConverter>();
            Map(m => m.Fees);
            Map(m => m.FullDescription);
            Map(m => m.Heading);
            Map(m => m.Illustration);
            Map(m => m.InitialInterestRate);
            Map(m => m.MaturityDate);
            Map(m => m.MaximumLtv);
            Map(m => m.MortgageTypes).TypeConverter<StringArrayConverter>();
            Map(m => m.Name);
            Map(m => m.ProductCode);
            Map(m => m.RateType).TypeConverter<StringArrayConverter>();
            Map(m => m.StandardVariableRate);
        }
    }
}
