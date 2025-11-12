using SpeedrunTracker.Localization;

namespace SpeedrunTracker.Services;
public abstract class BaseService
{
    protected static string Translate(string resourceKey)
    {
        return LocalizationResourceManager.Instance[resourceKey]?.ToString() ?? string.Empty;
    }
}
