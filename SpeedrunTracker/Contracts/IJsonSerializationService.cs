namespace SpeedrunTracker.Contracts;

public interface IJsonSerializationService
{
    string Serialize(object obj);

    T Deserialize<T>(string json);
}
