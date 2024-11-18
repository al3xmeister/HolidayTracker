using HolidayTracker.Services;

namespace HolidayTracker.ViewModels;

public partial class HomeViewModel(HolidayTrackerService service) : BaseViewModel
{
    private readonly HolidayTrackerService _service = service;
    [ObservableProperty]
    private int _remainingDaysAlex;

    [ObservableProperty]
    private int _remainingDaysElla;

    [ObservableProperty]
    private int _takenDaysAlex;

    [ObservableProperty]
    private int _takenDaysElla;

    public async Task LoadCalculations()
    {
        var getTotalTaken = await _service.GetAllHolidays();

        TakenDaysAlex = getTotalTaken.Where(d => d.Person == HolidayTracker.Enums.Person.Alex.ToString()).Sum(h => h.Taken);
        RemainingDaysAlex = 33 - TakenDaysAlex;

        TakenDaysElla = getTotalTaken.Where(d => d.Person == HolidayTracker.Enums.Person.Ella.ToString()).Sum(h => h.Taken);
        RemainingDaysElla = 16 - TakenDaysElla;
    }
}
