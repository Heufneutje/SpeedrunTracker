namespace SpeedrunTracker.Views;

public partial class BaseSearchContentPage : ContentPage
{
    public BaseSearchContentPage()
    {
        InitializeComponent();
    }

    protected void searchBar_SearchButtonPressed(object sender, EventArgs e)
    {
        (sender as SearchBar)?.Unfocus();
    }
}
