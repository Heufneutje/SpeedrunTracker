using SpeedrunTracker.Navigation;
using SpeedrunTracker.Views;

namespace SpeedrunTracker;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(Routes.GameDetailPageRoute, typeof(GameDetailPage));
        Routing.RegisterRoute(Routes.UserDetailPageRoute, typeof(UserDetailPage));
        Routing.RegisterRoute(Routes.RunDetailPageRoute, typeof(RunDetailsPage));
    }
}
