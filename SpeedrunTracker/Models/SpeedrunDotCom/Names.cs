﻿namespace SpeedrunTracker.Models.SpeedrunDotCom;

public record Names
{
    public required string International { get; set; }
    public string? Japanese { get; set; }
    public string? Twitch { get; set; }
}
