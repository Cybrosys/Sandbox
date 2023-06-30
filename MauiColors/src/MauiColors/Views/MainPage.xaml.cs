using MauiColors.ViewModels;

namespace MauiColors.Views;

public partial class MainPage : BaseContentPage<MainViewModel>
{
    public MainPage(MainViewModel viewModel)
        : base(viewModel)
    {
        InitializeComponent();
    }
}

public abstract class BaseContentPage<TViewModel> : BaseContentPage
    where TViewModel : class
{
    public BaseContentPage(TViewModel viewModel)
        : base(viewModel)
    {       
    }

    public new TViewModel BindingContext => (TViewModel)base.BindingContext;
}

public abstract class BaseContentPage : ContentPage
{
    public BaseContentPage(object viewModel = null)
    {
        BindingContext = viewModel;
    }
}