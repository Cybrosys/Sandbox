using CommunityToolkit.Maui.Converters;
using System.Globalization;

namespace MauiColors.Converters;

public class HexRgbStringToColorConverter : BaseConverterOneWay<string, Color>
{
    public override Color DefaultConvertReturnValue { get; set; } = Colors.Green;

    public override Color ConvertFrom(string value, CultureInfo culture)
    {
        return Color.FromHex(value);
    }
}
