using SpeedrunTracker.Model;
using System.Collections.ObjectModel;

namespace SpeedrunTracker.ViewModels;

public class VariableViewModel : BaseViewModel
{
    public string VariableId { get; set; }
    public string Name { get; set; }

    private ViewVariableValue _selectedValue;

    public ViewVariableValue SelectedValue
    {
        get => _selectedValue;
        set
        {
            _selectedValue = value;
            NotifyPropertyChanged();
        }
    }

    private ObservableCollection<ViewVariableValue> _values;

    public ObservableCollection<ViewVariableValue> Values
    {
        get => _values;
        set
        {
            _values = value;
            NotifyPropertyChanged();

            if (value.Any())
                SelectedValue = value.First();
        }
    }
}
