using HolidayTracker.Services;

namespace HolidayTracker.ViewModels
{
    public partial class ViewBookedViewModel(HolidayTrackerService service) : BaseViewModel
    {
        private readonly HolidayTrackerService _service = service;

        [ObservableProperty]
        private ObservableCollection<Holiday> _blocked;

        [ObservableProperty]
        private DateTime _today = DateTime.Today;

        public async Task LoadBookedDays()
        {
            var booked = await _service.GetAllHolidays();
            Blocked = new ObservableCollection<Holiday>(
                booked.OrderBy(d => d.StartDate)
            );
        }

        [RelayCommand]
        private void Edit(Holiday booked)
        {
            booked.Edit = !booked.Edit;
        }

        [RelayCommand]
        private async Task Delete(Holiday booked)
        {
            var result = await _service.DeleteHolidays(booked);

            if (result == 1)
            {
                Blocked.Remove(booked);
            }
        }

        [RelayCommand]
        private async Task Update(Holiday booked)
        {
            await _service.SaveHoliday(booked);
        }
    }
}
