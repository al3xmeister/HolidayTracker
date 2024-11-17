using HolidayTracker.Services;

namespace HolidayTracker.ViewModels;

public partial class HomeViewModel : BaseViewModel
{
    private readonly HolidayTrackerService _service;
    private int _currentMonth = DateTime.Today.Month;
    private int _currentYear = DateTime.Today.Year;

    [ObservableProperty]
    private string _currentMonthDisplay;

    public async Task LoadCurrentMonth()
    {
        CurrentMonthDisplay = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Today.Month);
    }

    public HomeViewModel(HolidayTrackerService service)
    {
        CurrentMonthDisplay = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(_currentMonth);
        _service = service;
    }

    [RelayCommand]
    private async Task LoadPreviousMonth()
    {
        if (_currentMonth > 1) _currentMonth--;
        if (_currentMonth == 1)
        {
            _currentMonth = 12;
            _currentYear--;
        }
        CurrentMonthDisplay = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(_currentMonth);
    }

    [RelayCommand]
    private async Task LoadNextMonth()
    {
        if (_currentMonth < 12)
        {
            _currentMonth++;
        }
        //if after December, start over
        else if (_currentMonth == 12)
        {
            _currentMonth = 1;
            _currentYear++;
        }
        CurrentMonthDisplay = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(_currentMonth);
    }
}
