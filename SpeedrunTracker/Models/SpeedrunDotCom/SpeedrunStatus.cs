using System.Text.Json.Serialization;

namespace SpeedrunTracker.Models.SpeedrunDotCom;

public record SpeedrunStatus
{
    [JsonPropertyName("status")]
    public SpeedrunStatusType StatusType { get; set; }

    [JsonPropertyName("examiner")]
    public string ExaminerId { get; set; }

    [JsonPropertyName("verify-date")]
    public DateTime? VerifyDate { get; set; }

    public string Reason { get; set; }
}
