using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using NIS.Desktop.Models;
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
        if (DataContext is MasterDataManagerViewModel vm)
        {
            // Get the item from the event source to handle case where selection hasn't updated yet
            var antenna = GetDataContextFromSource<Antenna>(e.Source);
            if (antenna != null)
            {
                vm.SelectedAntenna = antenna;
                vm.EditAntennaCommand.Execute(null);
            }
        }
    }

    private void OnCableDoubleTapped(object? sender, TappedEventArgs e)
    {
        if (DataContext is MasterDataManagerViewModel vm)
        {
            var cable = GetDataContextFromSource<Cable>(e.Source);
            if (cable != null)
            {
                vm.SelectedCable = cable;
                vm.EditCableCommand.Execute(null);
            }
        }
    }

    private void OnRadioDoubleTapped(object? sender, TappedEventArgs e)
    {
        if (DataContext is MasterDataManagerViewModel vm)
        {
            var radio = GetDataContextFromSource<Radio>(e.Source);
            if (radio != null)
            {
                vm.SelectedRadio = radio;
                vm.EditRadioCommand.Execute(null);
            }
        }
    }

    private void OnOkaDoubleTapped(object? sender, TappedEventArgs e)
    {
        if (DataContext is MasterDataManagerViewModel vm)
        {
            var oka = GetDataContextFromSource<Oka>(e.Source);
            if (oka != null)
            {
                vm.SelectedOka = oka;
                vm.EditOkaCommand.Execute(null);
            }
        }
    }

    private void OnTranslationDoubleTapped(object? sender, TappedEventArgs e)
    {
        if (DataContext is MasterDataManagerViewModel vm && vm.TranslationEditor.SelectedItem != null)
        {
            vm.TranslationEditor.StartEditCommand.Execute(null);
        }
    }

    private static T? GetDataContextFromSource<T>(object? source) where T : class
    {
        if (source is Control control)
        {
            // Walk up the visual tree to find the data context of type T
            var current = control;
            while (current != null)
            {
                if (current.DataContext is T item)
                    return item;
                current = current.GetVisualParent() as Control;
            }
        }
        return null;
    }
}
