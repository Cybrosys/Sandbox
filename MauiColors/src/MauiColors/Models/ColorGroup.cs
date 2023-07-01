using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace MauiColors.Models;

public partial class ColorGroup : ObservableObject
{
    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private ObservableCollection<Color> _colors = new ObservableCollection<Color>();
}
