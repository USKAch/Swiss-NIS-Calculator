using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using NIS.Desktop.Localization;

namespace NIS.Desktop.ViewModels;

public abstract class ViewModelBase : ObservableObject
{
    private bool _isDirty;

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

    /// <summary>
    /// Indicates if there are unsaved changes in this view.
    /// </summary>
    public bool IsDirty
    {
        get => _isDirty;
        set => SetProperty(ref _isDirty, value);
    }

    /// <summary>
    /// Callback to show a confirmation dialog. Set by MainShellViewModel.
    /// </summary>
    public Func<string, string, Task<bool>>? ShowConfirmDialog { get; set; }

    /// <summary>
    /// Marks the view as dirty (having unsaved changes).
    /// </summary>
    protected void MarkDirty() => IsDirty = true;

    /// <summary>
    /// Checks if navigation is allowed. If dirty, shows confirmation dialog.
    /// Returns true if navigation should proceed.
    /// </summary>
    protected async Task<bool> CanNavigateAwayAsync()
    {
        if (!IsDirty) return true;

        if (ShowConfirmDialog != null)
        {
            return await ShowConfirmDialog(
                Strings.Instance.DiscardChanges,
                Strings.Instance.DiscardChangesPrompt);
        }

        return true;
    }
}
