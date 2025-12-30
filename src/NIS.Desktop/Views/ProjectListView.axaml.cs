using Avalonia.Controls;
using Avalonia.Input;
using NIS.Desktop.Services;
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
        if (sender is DataGrid grid && grid.SelectedItem is ProjectListItem project)
        {
            if (DataContext is ProjectListViewModel vm)
            {
                vm.EditProjectCommand.Execute(project);
            }
        }
    }
}
