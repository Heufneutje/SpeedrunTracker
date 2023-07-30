﻿using SpeedrunTracker.Interfaces;
using SpeedrunTracker.Model;
using SQLite;

namespace SpeedrunTracker.Services;

public class LocalDatabaseService : ILocalDatabaseService
{
    public SQLiteAsyncConnection Connection { get; private set; }

    public async Task InitAsync(string databaseName, SQLiteOpenFlags flags)
    {
        if (Connection != null)
            return;

        Connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, databaseName), flags);
        await Connection.CreateTableAsync<FollowedEntity>();
    }
}
