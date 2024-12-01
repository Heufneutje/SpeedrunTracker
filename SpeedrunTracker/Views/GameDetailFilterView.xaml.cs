using SpeedrunTracker.ViewModels;

namespace SpeedrunTracker.Views;

public partial class GameDetailFilterView : ContentView
{
    public GameDetailFilterView()
    {
        InitializeComponent();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is GameDetailViewModel vm)
            await vm.LoadLeaderboardAsync();
    }
}
