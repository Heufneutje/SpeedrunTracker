using SpeedrunTracker.Model.Enum;
using System.Collections.ObjectModel;

namespace SpeedrunTracker.Model
{
    public class SearchResultGroup : ObservableCollection<Game>
    {
        public SearchType SearchType { get; }

        public string ImageSource
        {
            get
            {
                switch (SearchType)
                {
                    case SearchType.Games:
                        return "game.svg";
                    default:
                        return string.Empty;
                }
            }
        }

        public SearchResultGroup(SearchType searchType, List<Game> items) : base(items)
        {
            SearchType = searchType;
        }
    }
}
