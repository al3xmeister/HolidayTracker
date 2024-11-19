using HolidayTracker.Services;

namespace HolidayTracker.ViewModels
{
    public partial class ViewGiftDaysViewModel(HolidayTrackerService service) : BaseViewModel
    {
        private readonly HolidayTrackerService _service = service;

        [ObservableProperty]
        private ObservableCollection<GiftDay>? _giftDays;

        [ObservableProperty]
        private int _referenceYear = DateTime.Today.Year;

        public ObservableCollection<string> Persons { get; } =
        [
            Enums.Person.Alex.ToString(),
            Enums.Person.Ella.ToString(),
        ];

        [ObservableProperty]
        private string? _person;

        public async Task LoadGiftDays()
        {
            if (ReferenceYear >= DateTime.Today.AddYears(3).Year)
            {
                await Application.Current!.MainPage!.DisplayAlert("Neimplementat", "Pentru 3 ani+ nu putem calcula date", "Ok");
                return;
            }
            List<GiftDay> gifted = await _service.GetGiftedHolidays();
            var yearly = gifted.Where(d => d.Day.Year == ReferenceYear).ToList();
            GiftDays = new ObservableCollection<GiftDay>(yearly);
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
            var updated = await _service.SaveGiftDay(giftDay);

            if (updated == 0)
            {
                await Application.Current!.MainPage!.DisplayAlert("Error", "Unable to save", "Ok");
            }
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

        [RelayCommand]
        private async Task LoadPreviousYear()
        {
            ReferenceYear--;
            await LoadGiftDays();
        }

        [RelayCommand]
        private async Task LoadNextYear()
        {
            ReferenceYear++;
            await LoadGiftDays();
        }
    }
}
