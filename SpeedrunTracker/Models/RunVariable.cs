namespace SpeedrunTracker.Models;

public class RunVariable
{
    public string Name { get; set; }
    public string Value { get; set; }
    public bool IsSubcategory { get; set; }

    public RunVariable(string name, string value, bool isSubcategory)
    {
        Name = name;
        Value = value;
        IsSubcategory = isSubcategory;
    }
}
