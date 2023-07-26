using SpeedrunTracker.Model.Enum;
using System.Collections.ObjectModel;

namespace SpeedrunTracker.Model
{
    public class SearchResultGroup : ObservableCollection<SearchResult>
    {
        public SearchType SearchType { get; }

        public string ImageSource => SearchType switch
        {
            SearchType.Games => "game.svg",
            SearchType.Users => "user.svg",
            _ => string.Empty,
        };

        public SearchResultGroup(SearchType searchType, List<SearchResult> items) : base(items)
        {
            SearchType = searchType;
        }
    }
}
