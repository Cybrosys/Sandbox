﻿using CommunityToolkit.Maui;
using MauiColors.ViewModels;
using MauiColors.Views;
using Microsoft.Extensions.Logging;

namespace MauiColors;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		builder.Services.AddTransient<MainPage, MainViewModel>();
		builder.Services.AddTransient<ColorsPage, ColorsViewModel>();

		return builder.Build();
	}
}