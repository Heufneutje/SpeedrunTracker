﻿namespace SpeedrunTracker.Models;

public class Entity
{
    public string Id { get; set; }
    public string? Title { get; set; }
    public string? Subtitle { get; set; }
    public string? ImageUrl { get; set; }
    public object? SearchObject { get; set; }

    public Entity()
    {
        Id = string.Empty;
    }
}
