using SpeedrunTracker.Extensions;
using SpeedrunTracker.Model;
using SpeedrunTracker.Navigation;

namespace SpeedrunTracker.Views;

public partial class GamesView : ContentView
{
	public GamesView()
	{
		InitializeComponent();
	}

    private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        var test = e.Item;
        Game game = test as Game;
        await Shell.Current.GoToAsync(Routes.GameDetailPageRoute, "Game", game);
    }
}