using HolidayTracker.ViewModels;

namespace HolidayTracker.Pages;

public partial class ViewBookedPage : ContentPage
{
    public ViewBookedPage(ViewBookedViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ViewBookedViewModel viewModel)
        {
            await viewModel.LoadBookedDays();
        }
    }
}