using SpeedrunTracker.ViewModels;

namespace SpeedrunTracker.Views;

public partial class SearchPage : ContentPage
{
    public SearchPage(SearchEntityViewModel searchViewModel)
    {
        InitializeComponent();
        BindingContext = searchViewModel;
    }

    private void searchBar_SearchButtonPressed(object sender, EventArgs e)
    {
        (sender as SearchBar).Unfocus();
    }
}