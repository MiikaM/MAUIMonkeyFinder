using MAUIMonkeyFinder.Model;

namespace MAUIMonkeyFinder.View;

public partial class MainPage : ContentPage
{
    public MainPage(MonkeysViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}