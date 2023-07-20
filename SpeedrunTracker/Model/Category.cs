﻿using SpeedrunTracker.Model.Enum;

namespace SpeedrunTracker.Model;

public class Category : BaseSpeedrunObject
{
    public string Name { get; set; }
    public string Rules { get; set; }
    public bool Miscellaneous { get; set; }
    public CategoryType Type { get; set; }
}
