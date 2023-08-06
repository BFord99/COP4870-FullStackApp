using project2final.Models;

namespace project2final;


public partial class MainPage : ContentPage
{
    private MainViewModel _viewModel;

    public MainPage()
    {
        InitializeComponent();

        _viewModel = new MainViewModel();
        BindingContext = _viewModel;
    }
}

