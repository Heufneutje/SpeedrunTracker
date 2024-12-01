using System.Text.Json.Serialization;

namespace SpeedrunTracker.Models.Base
{
    public record BaseNamedSpeedrunModel : BaseSpeedrunModel
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }
    }
}
