using HolidayTracker.Data;
using System.Diagnostics;

namespace HolidayTracker;

public partial class App : Application
{
    private readonly HolidaysRepository _bookedDaysRepository;
    private readonly HolidaysRepository _holidaysRepository;
    public App(HolidaysRepository bookedDaysRepository, HolidaysRepository holidaysRepository)
	{
		InitializeComponent();

		_bookedDaysRepository = bookedDaysRepository;
        _holidaysRepository = holidaysRepository;
        MainPage = new AppShell();
	}

    protected override async void OnStart()
    {
        base.OnStart();

        Application.Current.UserAppTheme = AppTheme.Light;

        // Initialize the database asynchronously only once on app start
        try
        {
            await _bookedDaysRepository.SeedDataAsync();
            await _holidaysRepository.SeedDataAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error initializing the database: {ex.Message}");
        }
    }
}
