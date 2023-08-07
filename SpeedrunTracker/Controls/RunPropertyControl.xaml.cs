namespace SpeedrunTracker.Controls;

public partial class RunPropertyControl : ContentView
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(RunPropertyControl));
    public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(string), typeof(RunPropertyControl));
    public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(nameof(ImageSource), typeof(string), typeof(RunPropertyControl));
    public static readonly BindableProperty IsValueVisibleProperty = BindableProperty.Create(nameof(IsValueVisible), typeof(bool), typeof(RunPropertyControl));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string Value
    {
        get => (string)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public string ImageSource
    {
        get => (string)GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }

    public bool IsValueVisible
    {
        get => (bool)GetValue(IsValueVisibleProperty);
        set => SetValue(IsValueVisibleProperty, value);
    }

    public RunPropertyControl()
    {
        InitializeComponent();
        IsValueVisible = true;
    }
}
