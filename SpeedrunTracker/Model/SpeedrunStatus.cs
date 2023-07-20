using System.Text.Json.Serialization;

namespace SpeedrunTracker.Model;

public class SpeedrunStatus
{
    public string Status { get; set; }

    [JsonPropertyName("examiner")]
    public string ExaminerId { get; set; }

    [JsonPropertyName("verify-date")]
    public DateTime? VerifyDate { get; set; }
}
