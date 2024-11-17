namespace HolidayTracker.ViewModels
{
    public partial class InputHolidayViewModel(HolidaysRepository repository) : BaseViewModel
    {
        private readonly HolidaysRepository _repository = repository;

        [ObservableProperty]
        private string _inputName;

        [ObservableProperty]
        private DateTime _today = DateTime.Today;

        [ObservableProperty]
        private DateTime _startDate;

        [ObservableProperty]
        private DateTime _endDate;

        [ObservableProperty]
        private string _saveText;

        [RelayCommand]
        private async Task SaveAsync(ITextInput input)
        {
            var bookedDay = new Holidays
            {
                StartDate = StartDate,
                EndDate = EndDate,
                Name = InputName
            };

            var result = await _repository.SaveItemAsync(bookedDay);

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
