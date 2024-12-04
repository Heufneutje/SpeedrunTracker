using System.Collections.ObjectModel;
using SpeedrunTracker.ViewModels;

namespace SpeedrunTracker.Models;

public class UserPersonalBestsGroup : ObservableCollection<UserRunViewModel>
{
    public BaseGame Game { get; set; }

    public UserPersonalBestsGroup(BaseGame game, IEnumerable<UserRunViewModel> entries)
        : base(entries)
    {
        Game = game;
    }
}
