using SpeedrunTracker.Navigation;
using SpeedrunTracker.Pages;

namespace SpeedrunTracker;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(Routes.GameDetailPageRoute, typeof(GameDetailPage));
        Routing.RegisterRoute(Routes.RunDetailPageRoute, typeof(RunDetailsPage));
    }
}
