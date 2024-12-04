using System.Text.Json;
using System.Text.Json.Serialization;

namespace SpeedrunTracker.Services;

public class JsonSerializationService : IJsonSerializationService
{
    private readonly JsonSerializerOptions _options;

    public JsonSerializationService()
    {
        _options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumMemberConverter() },
        };
    }

    public string Serialize(object obj)
    {
        return JsonSerializer.Serialize(obj, _options);
    }

    public T? Deserialize<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json, _options);
    }
}
