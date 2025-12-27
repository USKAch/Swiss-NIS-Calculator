using NIS.Desktop.New.Services;

namespace NIS.Desktop.New.ViewModels;

/// <summary>
/// ViewModel for the Master Data view.
/// Handles navigation to antenna, cable, radio, and OKA management.
/// </summary>
public partial class MasterDataViewModel : ViewModelBase
{
    public MasterDataViewModel(ILocalizationService localization)
    {
        Loc = localization;
        SubscribeToLanguageChanges();
    }

    // TODO: Add commands for managing master data (antennas, cables, radios, OKAs)
}
