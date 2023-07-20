using SpeedrunTracker.ViewModels;

namespace SpeedrunTracker.Pages;

public partial class SearchPage : ContentPage
{
	public SearchPage(GameSearchViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    private void searchBar_SearchButtonPressed(object sender, EventArgs e)
    {
		(sender as SearchBar).Unfocus();
    }
}