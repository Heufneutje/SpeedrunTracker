using System.Collections.ObjectModel;

namespace SpeedrunTracker.Model;

public class UserPersonalBestsGroup : ObservableCollection<LeaderboardEntry>
{
    public BaseGame Game { get; set; }

    public UserPersonalBestsGroup(BaseGame game, List<LeaderboardEntry> entries) : base(entries)
    {
        Game = game;
    }
}
