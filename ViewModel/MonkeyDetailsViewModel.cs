using CommunityToolkit.Mvvm.ComponentModel;

namespace MAUIMonkeyFinder.ViewModel;

[QueryProperty(nameof(Monkey), "Monkey")]
public partial class MonkeyDetailsViewModel : BaseViewModel
{
   
    public MonkeyDetailsViewModel()
    {

    }

    [ObservableProperty]
    Monkey monkey; 
}