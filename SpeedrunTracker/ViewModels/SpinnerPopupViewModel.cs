﻿using CommunityToolkit.Mvvm.ComponentModel;

namespace SpeedrunTracker.ViewModels;

public partial class SpinnerPopupViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _loadingText = "Loading...";
}
