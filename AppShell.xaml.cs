using MAUIMonkeyFinder.View;

namespace MAUIMonkeyFinder;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(DetailsPage), typeof(DetailsPage));
	}
}
