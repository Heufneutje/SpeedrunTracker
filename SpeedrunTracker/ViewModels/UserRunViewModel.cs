namespace SpeedrunTracker.ViewModels;

public class UserRunViewModel
{
    public LeaderboardEntry Entry { get; set; }
    public string DateFormat { get; set; }
    public string FormattedDate => Entry.Run.Date?.ToString(DateFormat);

    public UserRunViewModel(LeaderboardEntry entry, string dateFormat)
    {
        Entry = entry;
        DateFormat = dateFormat;
    }
}
