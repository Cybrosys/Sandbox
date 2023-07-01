using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace MauiColors.ViewModels;

public partial class ColorsViewModel : ObservableObject
{
    [ObservableProperty]
    private List<Models.ColorGroup> _colorGroups = new List<Models.ColorGroup>()
    {
        new Models.ColorGroup { Name = "Colors" },
        //new Models.ColorGroup { Name = "Hue" },
        //new Models.ColorGroup { Name = "Luminosity" },
        //new Models.ColorGroup { Name = "Saturation" },
        new Models.ColorGroup { Name = "Other" },
        //new Models.ColorGroup { Name = "Red" },
        //new Models.ColorGroup { Name = "Orange" },
        //new Models.ColorGroup { Name = "Yellow" },
        //new Models.ColorGroup { Name = "Green" },
        //new Models.ColorGroup { Name = "Blue" },
        //new Models.ColorGroup { Name = "Indigo" },
        //new Models.ColorGroup { Name = "Violet" },
        //new Models.ColorGroup { Name = "Grayscale" },
        //new Models.ColorGroup { Name = "Off-white" },
    };

    [RelayCommand]
    private void ColorTapped(Models.Color color)
    {
        if (ColorGroups[0].Colors.Remove(color))
        {
            ColorGroups[1].Colors.Add(color);

            var colorsList = ColorGroups[1].Colors.Where(c => c is not null).OrderByDescending(c => Color.FromHex(c.Value).GetLuminosity()).ToList();
            for (int i = 0; i < colorsList.Count; ++i)
            {
                var fromIndex = ColorGroups[1].Colors.IndexOf(colorsList[i]);
                ColorGroups[1].Colors.Move(fromIndex, i);
            }
        }
        else if (ColorGroups[1].Colors.Remove(color))
        {
            ColorGroups[0].Colors.Add(color);
        }
    }

    [RelayCommand]
    private async Task LoadColorsAsync()
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync("Colors.json").ConfigureAwait(false);

        var colors = JsonSerializer.Deserialize<List<Models.Color>>(stream);
        ColorGroups[0].Colors = new ObservableCollection<Models.Color>(colors);

        //ColorGroups[0].Colors = new ObservableCollection<Models.Color>(colors.OrderBy(c => Color.FromHex(c.Value).GetHue()).ToList());
        //ColorGroups[1].Colors = new ObservableCollection<Models.Color>(colors.OrderByDescending(c => Color.FromHex(c.Value).GetLuminosity()).ToList());
        
        
        //ColorGroups[2].Colors = new ObservableCollection<Models.Color>(colors.OrderBy(c => Color.FromHex(c.Value).GetSaturation()).ToList());
        //ColorGroups[3].Colors = new ObservableCollection<Models.Color>(colors
        //    .OrderBy(c => Color.FromHex(c.Value).GetHue() * Color.FromHex(c.Value).GetSaturation() * Color.FromHex(c.Value).GetLuminosity())
        //    .ToList());

        //var colorRanges = new List<Tuple<string, ColorRange>>
        //{
        //    //Tuple.Create("Red", new ColorRange(Color.FromRgb(180, 0, 0), Color.FromRgb(255, 50, 50))),
        //    Tuple.Create("Red", new ColorRange(Color.FromRgb(70, 0, 0), Color.FromRgb(255, 70, 70))),
        //    Tuple.Create("Orange", new ColorRange(Color.FromRgb(255, 120, 0), Color.FromRgb(255, 180, 70))),
        //    Tuple.Create("Yellow", new ColorRange(Color.FromRgb(255, 200, 0), Color.FromRgb(255, 255, 100))),
        //    Tuple.Create("Green", new ColorRange(Color.FromRgb(0, 40, 0), Color.FromRgb(70, 255, 70))),
        //    Tuple.Create("Blue", new ColorRange(Color.FromRgb(0, 0, 40), Color.FromRgb(70, 70, 255))),
        //    Tuple.Create("Indigo", new ColorRange(Color.FromRgb(30, 0, 60), Color.FromRgb(80, 30, 120))),
        //    Tuple.Create("Violet", new ColorRange(Color.FromRgb(100, 0, 100), Color.FromRgb(150, 30, 150))),
        //};

        //var dumpGroup = ColorGroups.LastOrDefault();

        //while (colors.Count > 0)
        //{
        //    var color = colors[0];
        //    var mauiColor = Color.FromHex(color.Value);
        //    bool withinRange = false;

        //    if (color.Name == "Mars Red")
        //    { }

        //    foreach (var colorRange in colorRanges)
        //    {
        //        if (!colorRange.Item2.WithinRange(mauiColor))
        //            continue;

        //        withinRange = true;
        //        var colorGroup = ColorGroups.FirstOrDefault(g => g.Name == colorRange.Item1);
        //        colorGroup?.Colors.Add(color);
        //        break;
        //    }

        //    if (!withinRange)
        //        dumpGroup.Colors.Add(color);

        //    colors.Remove(color);
        //}
    }

    //class ColorRange
    //{
    //    private float[] _fromValues;
    //    private float[] _toValues;

    //    public ColorRange(Color from, Color to)
    //    {
    //        _fromValues = new float[] { from.Red, from.Green, from.Blue }; ;
    //        _toValues = new float[] { to.Red,  to.Green, to.Blue };
    //    }

    //    public bool WithinRange(Color color)
    //    {
    //        var colorValues = new float[] { color.Red, color.Green, color.Blue };

    //        for (int i = 0; i  < colorValues.Length; i++)
    //        {
    //            if (colorValues[i] < _fromValues[i] || colorValues[i] > _toValues[i])
    //                return false;
    //        }

    //        return true;
    //    }
    //}
}
