using HolidayTracker.ViewModels;

namespace HolidayTracker.Pages;

public partial class HomePage : ContentPage
{
    public HomePage(HomeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is HomeViewModel viewModel)
        {
            // Load booked days
            await viewModel.LoadCurrentMonth();
        }
    }
}