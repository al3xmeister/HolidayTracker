using HolidayTracker.Const;
using HolidayTracker.Services;

namespace HolidayTracker.ViewModels;

public partial class HomeViewModel(HolidayTrackerService service) : BaseViewModel
{
    private readonly HolidayTrackerService _service = service;
    [ObservableProperty]
    private double _remainingDaysAlex;

    [ObservableProperty]
    private double _remainingDaysElla;

    [ObservableProperty]
    private double _takenDaysAlex;

    [ObservableProperty]
    private double _takenDaysElla;

    [ObservableProperty]
    private double _daysUntilNextHoliday;

    [ObservableProperty]
    private DateTime _nextHolidayStart;

    [ObservableProperty]
    private DateTime _nextHolidayEnd;

    [ObservableProperty]
    private string? _nextHolidayName;

    [ObservableProperty]
    private DateTime _farApartHolidayStart;

    [ObservableProperty]
    private DateTime _farApartHolidayEnd;

    [ObservableProperty]
    private double _highestNoOfDaysApart;

    [ObservableProperty]
    private int _referenceYear = DateTime.Today.Year;

    [RelayCommand]
    private async Task LoadPreviousYear()
    {
        ReferenceYear--;
        await LoadCalculations();
    }

    [RelayCommand]
    private async Task LoadNextYear()
    {
        ReferenceYear++;
        await LoadCalculations();
    }

    public async Task LoadCalculations()
    {
        var today = DateTime.Now;

        if (ReferenceYear < today.Year)
        {
            await Application.Current!.MainPage!.DisplayAlert("Neimplementat", "Pentru anul trecut nu putem calcula date", "Ok");
            return;
        }

        var getAllHolidays = await _service.GetHolidaysByYear(ReferenceYear);
        var getCurrentYearHolidaysAlex = getAllHolidays.Where(d => d.Person == Enums.Person.Alex.ToString() && d.StartDate.Year == ReferenceYear);
        var getCurrentYearHolidaysElla = getAllHolidays.Where(d => d.Person == Enums.Person.Ella.ToString() && d.StartDate.Year == ReferenceYear);
        double takenDaysAlex = 0;
        foreach (var holiday in getCurrentYearHolidaysAlex)
        {
            var taken = await _service.CalculateDaysTakenForAlex(holiday);
            takenDaysAlex += taken;
        }
        TakenDaysAlex = takenDaysAlex;
        RemainingDaysAlex = HolidayAllowance.AllowanceForAlex - TakenDaysAlex;

        double takenDaysElla = 0;
        foreach (var holiday in getCurrentYearHolidaysElla)
        {
            var taken = await _service.CalculateDaysTakenForElla(holiday);
            takenDaysElla += taken;
        }
        TakenDaysElla = takenDaysElla;
        RemainingDaysElla = HolidayAllowance.AllowanceForElla - TakenDaysElla;

        var closestHoliday = getAllHolidays.OrderBy(obj => Math.Abs((obj.StartDate - today).Ticks))
            .FirstOrDefault();
        if (closestHoliday is not null)
        {
            DaysUntilNextHoliday = (closestHoliday.StartDate - today).TotalDays;
            NextHolidayStart = closestHoliday.StartDate;
            NextHolidayEnd = closestHoliday.EndDate;
            NextHolidayName = closestHoliday.Name;
        }
        else
        {
            DaysUntilNextHoliday = 365;
            var nextYearsFirstDay = new DateTime(DateTime.Now.Year + 1, 1, 1);
            NextHolidayStart = NextHolidayEnd = nextYearsFirstDay;
        }

        CalculateFurthestApart(getAllHolidays);
    }

    private void CalculateFurthestApart(List<Holiday> getAllHolidays)
    {
        var futureHolidays = getAllHolidays
                .Where(e => e.StartDate >= DateTime.Today) // Filter only future holidays
                .OrderBy(e => e.StartDate);

        var furthestApart = futureHolidays
        .Zip(futureHolidays.OrderBy(e => e.StartDate).Skip(1), (current, next) => new
        {
            CurrentEnd = current.EndDate,
            NextStart = next.StartDate,
            Gap = Math.Max(0, (next.StartDate - current.EndDate).TotalDays)
        })
        .OrderByDescending(g => g.Gap) // Order by the largest gap
        .FirstOrDefault(); // Get the furthest gap

        if (furthestApart is not null)
        {
            FarApartHolidayEnd = furthestApart.CurrentEnd;
            FarApartHolidayStart = furthestApart.NextStart;
            HighestNoOfDaysApart = furthestApart.Gap;
        }
    }
}
