using HolidayTracker.Services;

namespace HolidayTracker.ViewModels
{
    public partial class ViewHolidaysViewModel(HolidayTrackerService service) : BaseViewModel
    {
        private readonly HolidayTrackerService _service = service;

        [ObservableProperty]
        private ObservableCollection<Holiday> _holidays;

        [ObservableProperty]
        private DateTime _today = DateTime.Today;

        public async Task LoadHolidays()
        {
            var booked = await _service.GetAllHolidays();
            Holidays = new ObservableCollection<Holiday>(booked);
        }

        [RelayCommand]
        private void Edit(Holiday holiday)
        {
            holiday.Edit = !holiday.Edit;
        }

        [RelayCommand]
        private async Task Delete(Holiday holiday)
        {
            var result = await _service.DeleteHolidays(holiday);

            if (result == 1)
            {
                Holidays.Remove(holiday);
            }
        }

        [RelayCommand]
        private async Task Update(Holiday holiday)
        {
            await _service.SaveHoliday(holiday);
        }
    }
}
