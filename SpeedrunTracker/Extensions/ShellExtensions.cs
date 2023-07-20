namespace SpeedrunTracker.Extensions;

public static class ShellExtensions
{
    public static Task GoToAsync(this Shell shell, string route, string name, object property)
    {
        Dictionary<string, object> parameters = new() {{ name, property } };
        return shell.GoToAsync(route, parameters);
    }
}
