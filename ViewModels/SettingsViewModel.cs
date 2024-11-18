using HolidayTracker.Services;

namespace HolidayTracker.ViewModels;

public partial class SettingsViewModel(HolidayTrackerService service) : BaseViewModel
{
    private readonly HolidayTrackerService _service = service;

    [RelayCommand]
    private async Task ResetDatabase()
    {
        await _service.ResetDatabase();
    }
}
