using HolidayTracker.Const;
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

    [ObservableProperty]
    private double _daysUntilNextHoliday;

    [ObservableProperty]
    private DateTime _nextHolidayStart;

    [ObservableProperty]
    private DateTime _nextHolidayEnd;

    [ObservableProperty]
    private DateTime _farApartHolidayStart;

    [ObservableProperty]
    private DateTime _farApartHolidayEnd;

    [ObservableProperty]
    private double _highestNoOfDaysApart;
    public async Task LoadCalculations()
    {
        var today = DateTime.Today;
        var getAllHolidays = await _service.GetAllHolidays();
        var getCurrentYearHolidaysAlex = getAllHolidays.Where(d => d.Person == Enums.Person.Alex.ToString() && d.StartDate.Year == today.Year);
        var getCurrentYearHolidaysElla = getAllHolidays.Where(d => d.Person == Enums.Person.Ella.ToString() && d.StartDate.Year == today.Year);
        var takenDaysAlex = 0;
        foreach (var holiday in getCurrentYearHolidaysAlex)
        {
            var taken = _service.CalculateDaysTakenForAlex(holiday);
            takenDaysAlex += taken;
        }
        TakenDaysAlex = takenDaysAlex;
        RemainingDaysAlex = HolidayAllowance.AllowanceForAlex - TakenDaysAlex;

        var takenDaysElla = 0;
        foreach (var holiday in getCurrentYearHolidaysElla)
        {
            var taken = _service.CalculateDaysTakenForElla(holiday);
            takenDaysElla += taken;
        }
        TakenDaysElla = takenDaysElla;
        RemainingDaysElla = HolidayAllowance.AllowanceForElla - TakenDaysElla;

        var closestHoliday = getAllHolidays.OrderBy(obj => Math.Abs((obj.StartDate - today).Ticks))
            .FirstOrDefault();
        if (closestHoliday is not null)
        {
            DaysUntilNextHoliday = (closestHoliday.StartDate - today).Days;
            NextHolidayStart = closestHoliday.StartDate;
            NextHolidayEnd = closestHoliday.EndDate;
        }
        else
        {
            DaysUntilNextHoliday = 365;
            var nextYearsFirstDay = new DateTime(DateTime.Now.Year + 1, 1, 1);
            NextHolidayStart = NextHolidayEnd = nextYearsFirstDay;
        }

        var furthestApart = getAllHolidays
            .Where(e => e.StartDate >= DateTime.Today)
            .Zip(getAllHolidays.OrderBy(e => e.StartDate).Skip(1), (current, next) => new
            {
                CurrentEnd = current.EndDate,
                NextStart = next.StartDate,
                Gap = Math.Max(0, (next.StartDate - current.EndDate).TotalDays) 
            })
            .OrderByDescending(g => g.Gap) 
            .FirstOrDefault();
        if (furthestApart is not null)
        {
            FarApartHolidayEnd = furthestApart.CurrentEnd;
            FarApartHolidayStart = furthestApart.NextStart;
            HighestNoOfDaysApart = (double)furthestApart.Gap;
        }
    }
}
