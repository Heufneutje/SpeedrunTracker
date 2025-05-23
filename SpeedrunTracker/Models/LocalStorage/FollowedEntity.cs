﻿using SQLite;

namespace SpeedrunTracker.Models.LocalStorage
{
    public class FollowedEntity
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string? Title { get; set; }
        public string? Subtitle { get; set; }
        public string? ImageUrl { get; set; }
        public EntityType Type { get; set; }

        public FollowedEntity()
        {
            Id = string.Empty;
        }
    }
}
