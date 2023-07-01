using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MauiColors.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private int _count = 0;

    [ObservableProperty]
    private string _counterDescription = "Click me";

    [RelayCommand]
    public void IncrementCounter()
    {
        _count++;

        if (_count == 1)
            CounterDescription = $"Clicked {_count} time";
        else
            CounterDescription = $"Clicked {_count} times";

        SemanticScreenReader.Announce(CounterDescription);
    }
}
