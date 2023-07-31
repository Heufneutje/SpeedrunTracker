using System.Text.Json.Serialization;

namespace SpeedrunTracker.Model.SpeedrunDotCom;

public class SpeedrunStatus
{
    [JsonPropertyName("status")]
    public SpeedrunStatusType StatusType { get; set; }

    [JsonPropertyName("examiner")]
    public string ExaminerId { get; set; }

    [JsonPropertyName("verify-date")]
    public DateTime? VerifyDate { get; set; }

    public string Reason { get; set; }
}
