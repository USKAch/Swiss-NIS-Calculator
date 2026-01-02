using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using NIS.Desktop.Services;
using NIS.Desktop.Services.Repositories;
using NIS.Desktop.ViewModels;

namespace NIS.Desktop.Views;

public partial class ProjectListView : UserControl
{
    public ProjectListView()
    {
        InitializeComponent();
    }

    private void OnProjectDoubleTapped(object? sender, TappedEventArgs e)
    {
        if (DataContext is ProjectListViewModel vm)
        {
            // Get the item from the event source to handle case where selection hasn't updated yet
            var project = GetDataContextFromSource<ProjectListItem>(e.Source);
            if (project != null)
            {
                vm.EditProjectCommand.Execute(project);
            }
        }
    }

    private static T? GetDataContextFromSource<T>(object? source) where T : class
    {
        if (source is Control control)
        {
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
