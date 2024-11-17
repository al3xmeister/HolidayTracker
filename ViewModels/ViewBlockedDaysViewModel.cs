using HolidayTracker.Services;

namespace HolidayTracker.ViewModels
{
    public partial class ViewBookedViewModel(HolidayTrackerService service) : BaseViewModel
    {
        private readonly HolidayTrackerService _service = service;

        [ObservableProperty]
        private ObservableCollection<Holidays> _blocked;

        [ObservableProperty]
        private DateTime _today = DateTime.Today;

        public async Task LoadBookedDays()
        {
            var booked = await _service.GetHolidays();
            Blocked = new ObservableCollection<Holidays>(
                booked.OrderBy(d => d.StartDate)
            );
        }

        [RelayCommand]
        private void Edit(Holidays booked)
        {
            booked.Edit = !booked.Edit;
        }

        [RelayCommand]
        private async Task Delete(Holidays booked)
        {
            var result = await _service.DeleteHolidays(booked);

            if (result == 1)
            {
                Blocked.Remove(booked);
            }
        }

        [RelayCommand]
        private async Task Update(Holidays booked)
        {
            await _service.SaveHolidays(booked);
        }
    }
}
