using SpeedrunTracker.ViewModels;

namespace SpeedrunTracker.Views;

public partial class GameDetailView : ContentView
{
	public GameDetailView()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		GameDetailViewModel vm = BindingContext as GameDetailViewModel;
		await vm.LoadLeaderboardAsync();
    }
}