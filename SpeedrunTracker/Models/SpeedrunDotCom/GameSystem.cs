﻿using System.Text.Json.Serialization;

namespace SpeedrunTracker.Models.SpeedrunDotCom;

public record GameSystem
{
    [JsonPropertyName("platform")]
    public string PlatformId { get; set; }

    public bool Emulated { get; set; }

    [JsonPropertyName("region")]
    public string RegionId { get; set; }
}
