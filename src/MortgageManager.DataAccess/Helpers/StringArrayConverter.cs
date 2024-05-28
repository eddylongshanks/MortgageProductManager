using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace MortgageManager.DataAccess.Helpers;

public class StringArrayConverter : DefaultTypeConverter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrEmpty(text))
        {
            return Array.Empty<string>();
        }

        return text.Split(',');
    }
}