using CommunityToolkit.Mvvm.Input;
using MAUIMonkeyFinder.Services;
namespace MAUIMonkeyFinder.ViewModel;

public partial class MonkeysViewModel : BaseViewModel
{
    MonkeyService monkeyService;
    public ObservableCollection<Monkey> Monkeys { get; } = new();
    public MonkeysViewModel(MonkeyService monkeyService)
    {
        Title = "Monkey finder";
        this.monkeyService = monkeyService;
    }

    [RelayCommand]
    async Task GetMonkeysAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy= true;
            var monkeys = await monkeyService.GetMonkeys();
            if(Monkeys.Count() != 0) Monkeys.Clear();

            /// Maybe should use batch updates 
            foreach( var monkey in monkeys)
            {
                Monkeys.Add(monkey);
            }
        }
        catch (Exception ex)
        {
            // Maybe using an interface for this should be good
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error!", $"Unable to get monkeys: {ex.Message}", "Ok");
        }
        finally
        {
            IsBusy= false;
        }
    }

}
