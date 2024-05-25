
using Kontent.Ai.Management.Models.LanguageVariants.Elements;

namespace MortgageManager.CMS.Models
{
    public interface IProductMortgage
    {
        string Codename { get; }
        string[] ClientType { get; }
        string ComparisonCost { get; }
        string[] DealTerm { get; }
        string Fees { get; }
        string? FullDescription { get; }
        string? Heading { get; }
        string? Illustration { get; }
        string InitialInterestRate { get; }
        DateTime? MaturityDate { get; }
        string MaximumLtv { get; }
        string[] MortgageTypes { get; }
        string Name { get; }
        string PageCodename { get; }
        string ProductCode { get; }
        string[] RateType { get; }
        string StandardVariableRate { get; }        

        DateTimeElement GetDateTimeElementForMaturityDate();
        TextElement GetTextElementFor(string propertyName);
        RichTextElement GetRichTextElementFor(string propertyName);
        MultipleChoiceElement GetMultipleChoiceElementFor(string propertyName);
    }
}
