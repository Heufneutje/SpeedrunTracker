﻿namespace SpeedrunTracker.Model;

public class BaseGame : BaseSpeedrunObject
{
    public Names Names { get; set; }
    public GameAssets Assets { get; set; }
    public int Released { get; set; }
}
