using MauiColors.ViewModels;

namespace MauiColors.Views;

public partial class ColorsPage : BaseContentPage<ColorsViewModel>
{
	public ColorsPage(ColorsViewModel viewModel)
		: base(viewModel)
	{
		InitializeComponent();
	}
}