using HolidayTracker.Services;

namespace HolidayTracker.ViewModels
{
    public partial class ViewHolidaysViewModel(HolidayTrackerService service) : BaseViewModel
    {
        private readonly HolidayTrackerService _service = service;

        [ObservableProperty]
        private ObservableCollection<Holidays> _holidays;

        [ObservableProperty]
        private DateTime _today = DateTime.Today;

        public async Task LoadHolidays()
        {
            var booked = await _service.GetHolidays();
            Holidays = new ObservableCollection<Holidays>(booked);
        }

        [RelayCommand]
        private void Edit(Holidays holiday)
        {
            holiday.Edit = !holiday.Edit;
        }

        [RelayCommand]
        private async Task Delete(Holidays holiday)
        {
            var result = await _service.DeleteHolidays(holiday);

            if (result == 1)
            {
                Holidays.Remove(holiday);
            }
        }

        [RelayCommand]
        private async Task Update(Holidays holiday)
        {
            await _service.SaveHolidays(holiday);
        }
    }
}
