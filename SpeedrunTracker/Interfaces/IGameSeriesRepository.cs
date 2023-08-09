﻿namespace SpeedrunTracker.Interfaces;

public interface IGameSeriesRepository
{
    Task<PagedData<List<GameSeries>>> SearchGameSeriesAsync(string name);

    Task<BaseData<GameSeries>> GetGameSeriesAsync(string seriesId);

    Task<PagedData<List<Game>>> GetGameSeriesEntriesAsync(string seriesId, int offset);
}
