using CommunityToolkit.Mvvm.ComponentModel;
using NIS.Desktop.New.Localization;

namespace NIS.Desktop.New.ViewModels;

public abstract class ViewModelBase : ObservableObject
{
    public ViewModelBase()
    {
        // Subscribe to language changes and notify that Strings property changed
        Strings.Instance.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(Strings.Language) || e.PropertyName == null)
            {
                OnPropertyChanged(nameof(Strings));
            }
        };
    }

    /// <summary>
    /// Provides access to localized strings for data binding.
    /// </summary>
    public Strings Strings => Strings.Instance;
}
