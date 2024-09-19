
namespace MortgageManager.CMS.Helpers
{
    internal class ValueConverters
    {
        private string ClientTypeOptions => $"N, Y, both";
        private string RateTypeOptions => $"Fixed, Variable";
        private string DealTermsOptions => $"1, 2, 3, 5, 10";
        private string MortgageTypesOptions => $"fill this in hew";

        public string[] ConvertClientTypes(string? clientType) => clientType switch
        {
            "N" => ["new"],
            "Y" => ["existing"],
            "both" => ["new", "existing"],
            null => [""],
            _ => throw new ArgumentException($"""'ClientType' value was invalid.{Environment.NewLine}Valid options are: {ClientTypeOptions}{Environment.NewLine}"""),
        };

        public string ConvertRateTypes(string? rateType) => rateType switch
        {
            "Fixed" => "fixed",
            "Variable" => "variable",
            null => "",
            _ => throw new ArgumentException($"""'RateType' value was invalid.{Environment.NewLine}Valid options are: {RateTypeOptions}{Environment.NewLine}"""),
        };

        public string ConvertDealTerms(string? dealTerm) => dealTerm switch
        {
            "1" => "one_year",
            "2" => "two_years",
            "3" => "three_years",
            "5" => "five_years",
            "10" => "ten_years",
            null => "",
            _ => throw new ArgumentException($"""'DealTerm' value was invalid.{Environment.NewLine}Valid options are: {DealTermsOptions}{Environment.NewLine}"""),
        };

        public string[] ConvertMortgageTypes(string? mortgageTypes) => mortgageTypes switch
        {
            // todo: needs conversion mapping
            "Residential" => ["standard"],
            "Residential - LISA Exclusive" => ["standard"],
            "Residential - NE Exclusive" => ["standard"],
            "Residential - Discounted Rate" => ["standard"],
            "Residential - Interest Only" => ["standard"],
            "Residential - Remortgage" => ["standard"],
            "Large Loan" => ["standard"],
            "Buy to Let" => ["standard"],
            "Self Employed" => ["standard"],
            "Help to Buy" => ["standard"],
            "Joint Mortgage Sole Proprietor" => ["standard"],
            "Residential - Existing Customer only" => ["standard"],
            "Residential - Interest Only - Existing Customer only" => ["standard"],
            "Large Loan - Existing Customer only" => ["standard"],
            "Buy to Let - Existing Customer only" => ["standard"],
            "Help to Buy - Existing Customer only" => ["standard"],
            "Joint Mortgage Sole Proprietor - Existing Customer only" => ["standard"],
            "Self Build - B / Store" => ["standard"],
            "Self Build - B / Loan" => ["standard"],
            "Custom Build - B / Store" => ["standard"],
            "Custom Build - B / Loan" => ["standard"],
            "Accelerate Custom Build B / Store" => ["standard"],
            "Accelerate Custom Build B / Loan" => ["standard"],
            "Accelerate Self Build B / Store" => ["standard"],
            "Accelerate Self Build B / Loan" => ["standard"],
            "Deposit Unlock" => ["standard"],
            "Government First Home Scheme" => ["standard"],
            "Shared Ownership" => ["standard"],
            null => [""],
            _ => throw new ArgumentException($"""'MortgageTypes' value was invalid.{Environment.NewLine}Valid options are: {MortgageTypesOptions}{Environment.NewLine}"""),
        };
    }
}
