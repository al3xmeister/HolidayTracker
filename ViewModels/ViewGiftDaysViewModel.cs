using HolidayTracker.Services;

namespace HolidayTracker.ViewModels
{
    public partial class ViewGiftDaysViewModel(HolidayTrackerService service) : BaseViewModel
    {
        private readonly HolidayTrackerService _service = service;

        [ObservableProperty]
        private ObservableCollection<GiftDay>? _giftDays;

        [ObservableProperty]
        private DateTime _today = DateTime.Today;

        public async Task LoadGiftDays()
        {
            List<GiftDay> gifted = await _service.GetGiftedHolidays();
            GiftDays = new ObservableCollection<GiftDay>(gifted);
        }

        [RelayCommand]
        private void Edit(GiftDay giftDay)
        {
            giftDay.Edit = !giftDay.Edit;
        }

        [RelayCommand]
        private async Task Delete(GiftDay giftDay)
        {
            var result = await _service.DeleteGiftDay(giftDay);

            if (result == 1)
            {
                GiftDays?.Remove(giftDay);
            }
        }

        [RelayCommand]
        private async Task Update(GiftDay giftDay)
        {
            await _service.SaveGiftDay(giftDay);
        }

        [RelayCommand]
        private void Add()
        {
            GiftDays?.Add(new GiftDay
            {
                Day = DateTime.Today,
                Edit = true,
                Name = "Extra",
            });
        }
    }
}
