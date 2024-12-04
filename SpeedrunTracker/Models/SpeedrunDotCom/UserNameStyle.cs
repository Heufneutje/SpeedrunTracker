using System.Text.Json.Serialization;

namespace SpeedrunTracker.Models.SpeedrunDotCom;

public record UserNameStyle
{
    public UserNameStyleType Style { get; set; }

    [JsonPropertyName("color")]
    public UserNameStyleColor? SolidColor { get; set; }

    [JsonPropertyName("color-from")]
    public UserNameStyleColor? ColorFrom { get; set; }

    [JsonPropertyName("color-to")]
    public UserNameStyleColor? ColorTo { get; set; }

    [JsonIgnore]
    public Color ThemeColor =>
        Color.FromArgb(
            Style == UserNameStyleType.Gradient
                ? (Application.Current?.RequestedTheme == AppTheme.Dark ? ColorFrom?.Dark : ColorFrom?.Light)
                : (Application.Current?.RequestedTheme == AppTheme.Dark ? SolidColor?.Dark : SolidColor?.Light)
        );
}
