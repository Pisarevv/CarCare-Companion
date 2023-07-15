namespace CarCare_Companion.Common;

using static GlobalConstants;

public static class FormattingMethods
{
    public static string FormatDateTimeToString(DateTime inputDatetime)
    {
        return inputDatetime.ToString(DefaultTimeFormat);
    }
}