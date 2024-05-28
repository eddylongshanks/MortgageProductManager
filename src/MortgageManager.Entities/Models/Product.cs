
using MortgageManager.Entities.Helpers;

namespace MortgageManager.Entities.Models
{
    public class Product
    {
        public string APRC { get; set; }
        public string[] ClientType { get; set; }
        public string[] DealTerm { get; set; }
        public string Fees { get; set; }
        public string? FullDescription { get; set; }
        public string Heading { get; set; }
        public string? Illustration { get; set; }
        public string InitialInterestRate { get; set; }
        public DateTime? MaturityDate { get; set; }
        public string MaximumLtv { get; set; }
        public string[] MortgageTypes { get; set; }
        public string? Name { get; set; }
        public string ProductCode { get; set; }
        public string[] RateType { get; set; }
        public string StandardVariableRate { get; set; }
        public ProductStatus Status { get; set; } = ProductStatus.Unprocessed;
    }
}