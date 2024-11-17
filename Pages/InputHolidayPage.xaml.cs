using HolidayTracker.ViewModels;

namespace HolidayTracker.Pages;

public partial class InputHolidayPage : ContentPage
{
    private readonly InputHolidayViewModel _viewModel;

    public InputHolidayPage(InputHolidayViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        _viewModel.SaveText = "Save";
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.OnAppearing();
    }
}