using HolidayTracker.Services;

namespace HolidayTracker.ViewModels
{
    public partial class ViewBookedViewModel(HolidayTrackerService service) : BaseViewModel
    {
        private readonly HolidayTrackerService _service = service;

        [ObservableProperty]
        private ObservableCollection<Holiday>? _booked;

        [ObservableProperty]
        private int _referenceYear = DateTime.Today.Year;

        public async Task LoadBookedDays()
        {

            var booked = await _service.GetHolidaysByYear(ReferenceYear);

            for (int i = 0; i < booked.Count; i++)
            {
                Holiday? book = booked[i];
                if (book.NumberOfDaysTaken == default)
                {
                    book.NumberOfDaysTaken = await _service.CalculateDaysTakenForAlex(book);
                    await _service.SaveHoliday(book);
                }
            }

            Booked = new ObservableCollection<Holiday>(
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
                Booked?.Remove(booked);
            }
        }

        [RelayCommand]
        private async Task Update(Holiday booked)
        {
            await _service.SaveHoliday(booked);
        }

        [RelayCommand]
        private async Task LoadPreviousYear()
        {
            ReferenceYear--;
            await LoadBookedDays();
        }

        [RelayCommand]
        private async Task LoadNextYear()
        {
            ReferenceYear++;
            await LoadBookedDays();
        }
    }
}
