
namespace MortgageManager.CMS.Helpers
{
    internal class ValueConverters
    {
        private string ClientTypeOptions => $"new_customer, existing_customer";
        private string RateTypeOptions => $"fixed_rate, discounted_variable_rate, base_rate_tracker";
        private string DealTermsOptions => $"1, 2, 3, 5, 10";

        public string ConvertClientTypes(string? clientType) => clientType switch
        {
            "new_customer" => "new",
            "existing_customer" => "existing",
            null => "",
            _ => throw new ArgumentException($"""The provided value of "{nameof(clientType)}" was invalid. Valid options are: {ClientTypeOptions}"""),
        };

        public string ConvertRateTypes(string? rateType) => rateType switch
        {
            "fixed_rate" => "fixed",
            "discounted_variable_rate" => "variable",
            "base_rate_tracker" => "tracker",
            null => "",
            _ => throw new ArgumentException($"""The provided value of "{nameof(rateType)}" was invalid. Valid options are: {RateTypeOptions}"""),
        };

        public string ConvertDealTerms(string? dealTerm) => dealTerm switch
        {
            "1" => "one_year",
            "2" => "two_years",
            "3" => "three_years",
            "5" => "five_years",
            "10" => "ten_years",
            null => "",
            _ => throw new ArgumentException($"""The provided value of "{nameof(dealTerm)}" was invalid. Valid options are: {DealTermsOptions}"""),
        };
    }
}
