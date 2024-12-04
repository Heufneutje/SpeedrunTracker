namespace SpeedrunTracker.Models;

public class ExclusionCollection : Dictionary<string, List<string>>
{
    public static ExclusionCollection ParseExclusions(string configuredExclusions)
    {
        if (configuredExclusions == null)
            return [];

        return configuredExclusions
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(exclusion => exclusion.Split(':').Select(x => x.Trim()).ToArray())
            .Aggregate(
                new ExclusionCollection(),
                (exclusions, parts) =>
                {
                    if (!exclusions.ContainsKey(parts[0]))
                        exclusions[parts[0]] = [];
                    exclusions[parts[0]].Add(parts[1]);
                    return exclusions;
                }
            );
    }

    public bool IsExcluded(string gameId, string itemId) => ContainsKey(gameId) && this[gameId].Contains(itemId);
}
