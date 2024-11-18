using HolidayTracker.Services;

namespace HolidayTracker.ViewModels
{
    public partial class InputHolidayViewModel(HolidayTrackerService service) : BaseViewModel
    {
        private readonly HolidayTrackerService _service = service;

        [ObservableProperty]
        private string? _inputName;

        [ObservableProperty]
        private DateTime _today = DateTime.Today;

        [ObservableProperty]
        private DateTime _startDate;

        [ObservableProperty]
        private DateTime _endDate;

        [ObservableProperty]
        private string? _saveText;

        [ObservableProperty]
        private string? _person;

        [ObservableProperty]
        private string? _status;

        public ObservableCollection<string> Persons { get; } =
        [
            Enums.Person.Alex.ToString(),
            Enums.Person.Ella.ToString(),
        ];

        public ObservableCollection<string> Statuses { get; } =
        [
            Enums.Status.Aprobat.ToString(),
            Enums.Status.AprobatDoarInTimetastic.ToString(),
            Enums.Status.CerutPentruAprobare.ToString(),
            Enums.Status.Necerut.ToString(),
        ];

        [RelayCommand]
        private async Task SaveAsync(ITextInput input)
        {
            var booked = new Holiday
            {
                StartDate = StartDate,
                EndDate = EndDate,
                Name = InputName,
                Person = Person,
                Status = Status
            };

            var result = await _service.SaveHoliday(booked);

            if (result == 1)
            {
                await KeyboardExtensions.HideKeyboardAsync(input);
                SaveText = "Saved";
            }
        }

        public void OnAppearing()
        {
            SaveText = "Save";
        }
    }
}
