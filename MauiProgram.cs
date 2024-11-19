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
    
#if DEBUG
        builder.Logging.AddDebug();
#endif
        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HolidayTracker.db3");
        builder.Services.AddSingleton(_ => new SQLiteAsyncConnection(dbPath));

        builder.Services.AddSingleton<HolidayRepository>();
        builder.Services.AddSingleton<GiftDayRepository>();
        builder.Services.AddSingleton<HolidayTrackerService>();

        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<HomePage>();

        builder.Services.AddTransient<InputHolidayViewModel>();
        builder.Services.AddTransient<InputHolidayPage>();

        builder.Services.AddTransient<ViewBookedPage>();
        builder.Services.AddTransient<ViewBookedViewModel>();

        builder.Services.AddTransient<ViewGiftDaysPage>();
        builder.Services.AddTransient<ViewGiftDaysViewModel>();

        builder.Services.AddTransient<SettingsPage>();
        builder.Services.AddTransient<SettingsViewModel>();
        return builder.Build();
    }
}
