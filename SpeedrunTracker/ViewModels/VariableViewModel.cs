using CommunityToolkit.Mvvm.ComponentModel;

namespace SpeedrunTracker.ViewModels;

public partial class VariableViewModel : BaseViewModel, INamedSpeedrunModel
{
    public required string VariableId { get; set; }
    public required string Name { get; set; }

    [ObservableProperty]
    private ViewVariableValue? _selectedValue;

    [ObservableProperty]
    private List<ViewVariableValue>? _values;

    partial void OnValuesChanged(List<ViewVariableValue>? value)
    {
        if (value?.Count > 0)
            SelectedValue = value[0];
    }
}
