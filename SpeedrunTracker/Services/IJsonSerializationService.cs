namespace SpeedrunTracker.Services;

public interface IJsonSerializationService
{
    string Serialize(object obj);

    T Deserialize<T>(string json);
}
