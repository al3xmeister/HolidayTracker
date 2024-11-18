using CommunityToolkit.Maui;
using HolidayTracker.Pages;
using HolidayTracker.Services;
using HolidayTracker.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;

namespace HolidayTracker;

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
                fonts.AddFont(filename: "FontAwesome6Solid.otf", alias: "FontAwesome6");
            });

        // Modify the EntryHandler for all Entry controls
        EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
  {
#if ANDROID
            // Remove underline on Android
            handler.PlatformView.Background = null;
#elif IOS
      // Remove border on iOS
      handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#elif WINDOWS
            // Remove border on Windows
            handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
#endif
  });
#if DEBUG
        builder.Logging.AddDebug();
#endif
        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HolidayTracker.db3");
        builder.Services.AddSingleton(_ => new SQLiteAsyncConnection(dbPath));

        builder.Services.AddSingleton<HolidayRepository>();
        builder.Services.AddSingleton<HolidayTrackerService>();

        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<HomePage>();

        builder.Services.AddTransient<InputHolidayViewModel>();
        builder.Services.AddTransient<InputHolidayPage>();

        builder.Services.AddTransient<ViewBookedPage>();
        builder.Services.AddTransient<ViewBookedViewModel>();

        builder.Services.AddTransient<SettingsPage>();
        builder.Services.AddTransient<SettingsViewModel>();
        return builder.Build();
    }
}
