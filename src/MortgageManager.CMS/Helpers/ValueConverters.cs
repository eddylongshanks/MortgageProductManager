﻿
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
            _ => throw new ArgumentException($"""'Client Type' value was invalid.{Environment.NewLine}Valid options are: {ClientTypeOptions}{Environment.NewLine}"""),
        };

        public string ConvertRateTypes(string? rateType) => rateType switch
        {
            "fixed_rate" => "fixed",
            "discounted_variable_rate" => "variable",
            "base_rate_tracker" => "tracker",
            null => "",
            _ => throw new ArgumentException($"""'Rate Type' value was invalid.{Environment.NewLine}Valid options are: {RateTypeOptions}{Environment.NewLine}"""),
        };

        public string ConvertDealTerms(string? dealTerm) => dealTerm switch
        {
            "1" => "one_year",
            "2" => "two_years",
            "3" => "three_years",
            "5" => "five_years",
            "10" => "ten_years",
            null => "",
            _ => throw new ArgumentException($"""'Term' value was invalid.{Environment.NewLine}Valid options are: {DealTermsOptions}{Environment.NewLine}"""),
        };
    }
}
