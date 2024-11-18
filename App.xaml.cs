using HolidayTracker.Data;
using HolidayTracker.Services;
using System.Diagnostics;

namespace HolidayTracker;

public partial class App : Application
{
    private readonly HolidayTrackerService _service;
    public App(HolidayTrackerService service)
    {
        InitializeComponent();

        _service = service;
        MainPage = new AppShell();
    }

    protected override async void OnStart()
    {
        base.OnStart();

        Application.Current.UserAppTheme = AppTheme.Light;

        // Initialize the database asynchronously only once on app start
        try
        {
            await _service.SeedDataAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error initializing the database: {ex.Message}");
        }
    }
}
