using System.Windows.Input;

namespace SpeedrunTracker.Controls;

public partial class AvatarControl : ContentView
{
    public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(
        nameof(ImageSource),
        typeof(string),
        typeof(AvatarControl)
    );

    public static readonly BindableProperty CommandProperty = BindableProperty.Create(
        nameof(Command),
        typeof(ICommand),
        typeof(AvatarControl)
    );

    public static readonly BindableProperty AvatarSizeProperty = BindableProperty.Create(
        nameof(AvatarSize),
        typeof(double),
        typeof(AvatarControl)
    );

    public string ImageSource
    {
        get => (string)GetValue(ImageSourceProperty) ?? "user";
        set => SetValue(ImageSourceProperty, value);
    }

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public double AvatarSize
    {
        get => (double)GetValue(AvatarSizeProperty);
        set => SetValue(AvatarSizeProperty, value);
    }

    public int ImageMargin => !string.IsNullOrEmpty(ImageSource) ? 0 : 10;

    public AvatarControl()
    {
        InitializeComponent();
    }
}
