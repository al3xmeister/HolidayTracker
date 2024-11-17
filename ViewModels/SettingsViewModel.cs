using HolidayTracker.Services;

namespace HolidayTracker.ViewModels;

public partial class SettingsViewModel(HolidayTrackerService service) : BaseViewModel
{
    private HolidayTrackerService _service = service;

    [ObservableProperty]
    private bool _strictMode = true;

    partial void OnStrictModeChanged(bool oldValue, bool newValue)
    {
        Preferences.Set("StrictMode", StrictMode);
    }

    [RelayCommand]
    private async Task ResetDatabase()
    {
        await _service.ResetDatabase();
    }
}
