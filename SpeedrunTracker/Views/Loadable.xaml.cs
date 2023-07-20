namespace SpeedrunTracker.Views;

public partial class Loadable : ContentView
{
	public Loadable()
	{
		InitializeComponent();
	}

    public BindableProperty LoadableContentProp = BindableProperty.Create(nameof(LoadableContent), typeof(IView), typeof(Loadable));

    public IView LoadableContent
    {
        get => (IView)GetValue(LoadableContentProp);
        set => SetValue(LoadableContentProp, value);
    }
}