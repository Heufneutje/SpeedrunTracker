﻿namespace SpeedrunTracker.Models;

public record RunDetails
{
    public int Place { get; set; }
    public required Speedrun Run { get; set; }
    public Category? Category { get; set; }
    public Level? Level { get; set; }
    public GamePlatform? Platform { get; set; }
    public List<RunVariable> Variables { get; set; }
    public GameAssets? GameAssets { get; set; }
    public Ruleset? Ruleset { get; set; }
    public User? Examiner { get; set; }

    private string? _ordinalPlace;

    public string OrdinalPlace
    {
        get
        {
            _ordinalPlace ??= Place.AsOrdinal();
            return _ordinalPlace;
        }
    }

    private Asset? _trophyAsset;

    public Asset? TrophyAsset
    {
        get
        {
            if (_trophyAsset is null && Place < 5)
                _trophyAsset = GameAssets?.GetTrophyAsset(Place);
            return _trophyAsset;
        }
    }

    public RunDetails()
    {
        Variables = [];
    }
}
