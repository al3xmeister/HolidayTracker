using HolidayTracker.ViewModels;

namespace HolidayTracker.Pages;

public partial class ViewGiftDaysPage : ContentPage
{
    public ViewGiftDaysPage(ViewGiftDaysViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ViewGiftDaysViewModel viewModel)
        {
            await viewModel.LoadGiftDays();
        }
    }
}