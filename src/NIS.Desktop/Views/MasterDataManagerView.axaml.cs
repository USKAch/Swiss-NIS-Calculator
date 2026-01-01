using Avalonia.Controls;
using Avalonia.Input;
using NIS.Desktop.ViewModels;

namespace NIS.Desktop.Views;

public partial class MasterDataManagerView : UserControl
{
    public MasterDataManagerView()
    {
        InitializeComponent();
    }

    private void OnAntennaDoubleTapped(object? sender, TappedEventArgs e)
    {
        if (DataContext is MasterDataManagerViewModel vm && vm.SelectedAntenna != null)
        {
            vm.EditAntennaCommand.Execute(null);
        }
    }

    private void OnCableDoubleTapped(object? sender, TappedEventArgs e)
    {
        if (DataContext is MasterDataManagerViewModel vm && vm.SelectedCable != null)
        {
            vm.EditCableCommand.Execute(null);
        }
    }

    private void OnRadioDoubleTapped(object? sender, TappedEventArgs e)
    {
        if (DataContext is MasterDataManagerViewModel vm && vm.SelectedRadio != null)
        {
            vm.EditRadioCommand.Execute(null);
        }
    }

    private void OnOkaDoubleTapped(object? sender, TappedEventArgs e)
    {
        if (DataContext is MasterDataManagerViewModel vm && vm.SelectedOka != null)
        {
            vm.EditOkaCommand.Execute(null);
        }
    }

    private void OnTranslationDoubleTapped(object? sender, TappedEventArgs e)
    {
        if (DataContext is MasterDataManagerViewModel vm && vm.TranslationEditor.SelectedItem != null)
        {
            vm.TranslationEditor.StartEditCommand.Execute(null);
        }
    }
}
