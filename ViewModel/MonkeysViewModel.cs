using CommunityToolkit.Mvvm.Input;
using MAUIMonkeyFinder.Services;
using MAUIMonkeyFinder.View;
using Microsoft.Maui.Devices.Sensors;

namespace MAUIMonkeyFinder.ViewModel;

public partial class MonkeysViewModel : BaseViewModel
{
    MonkeyService monkeyService;
    public ObservableCollection<Monkey> Monkeys { get; } = new();
    public MonkeysViewModel(MonkeyService monkeyService, IConnectivity connectivity, IGeolocation geolocation)
    {
        Title = "Monkey finder";
        this.monkeyService = monkeyService;
        this.connectivity = connectivity;
        this.geolocation = geolocation;
    }

    IConnectivity connectivity;
    IGeolocation geolocation;

    [RelayCommand]
    async Task GetClosestMonkey()
    {
        if (IsBusy ||Monkeys.Count== 0) return;

        try
        {
            var location = await geolocation.GetLastKnownLocationAsync();
            if (location == null)
            {
                location = await geolocation.GetLocationAsync(
                    new GeolocationRequest
                    {
                         DesiredAccuracy= GeolocationAccuracy.Medium,
                         Timeout= TimeSpan.FromSeconds(30),
                    });
            }

            if (location is null) return;

            var closestMonkey = Monkeys.OrderBy(x => location.CalculateDistance(x.Latitude, x.Longitude, DistanceUnits.Kilometers)).FirstOrDefault();

            if (closestMonkey is null) return;

            await Shell.Current.DisplayAlert("Closest monkey", $"{closestMonkey.Name} in {closestMonkey.Location}", "Ok");
        }
        catch (Exception ex)
        {
            // Maybe using an interface for this should be good
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error!", $"Unable to get closest monkeys", "Ok");
        }
    }

    [RelayCommand]
    async Task GoToDetailsAsync(Monkey monkey)
    {
        if (monkey is null)
        {
            return;
        }

        await Shell.Current.GoToAsync($"{nameof(DetailsPage)}", true, new Dictionary<string, object>
        {
            {"Monkey", monkey }
        });
    }

    [RelayCommand]
    async Task GetMonkeysAsync()
    {
        if (IsBusy) return;

        try
        {
            if (connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                // Maybe using an interface for this should be good
                await Shell.Current.DisplayAlert("Internet issue", $"Check your internet and try again", "Ok");
                return;
            }

            IsBusy = true;
            var monkeys = await monkeyService.GetMonkeys();
            if (Monkeys.Count() != 0) Monkeys.Clear();

            foreach (var monkey in monkeys)
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
            IsBusy = false;
        }
    }

}
