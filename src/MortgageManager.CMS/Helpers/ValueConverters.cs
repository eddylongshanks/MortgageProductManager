using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MortgageManager.CMS.Helpers
{
    internal class ValueConverters
    {
        public string ConvertClientTypes(string? clientType) => clientType switch
        {
            "new_customer" => "new",
            "existing_customer" => "existing",
            null => "",
            _ => throw new ArgumentException($"""The provided value of "{nameof(clientType)}" was invalid"""),
        };

        public string ConvertRateTypes(string? rateType) => rateType switch
        {
            "fixed_rate" => "fixed",
            "discounted_variable_rate" => "variable",
            "base_rate_tracker" => "tracker",
            null => "",
            _ => throw new ArgumentException($"""The provided value of "{nameof(rateType)}" was invalid"""),
        };

        public string ConvertDealTerms(string? dealTerm) => dealTerm switch
        {
            "1" => "one_year",
            "2" => "two_years",
            "3" => "three_years",
            "5" => "five_years",
            "10" => "ten_years",
            null => "",
            _ => throw new ArgumentException($"""The provided value of "{nameof(dealTerm)}" was invalid"""),
        };
    }
}
