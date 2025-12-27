using System.ComponentModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using NIS.Desktop.ViewModels;

namespace NIS.Desktop.Views;

public partial class MainShellWindow : Window
{
    public MainShellWindow()
    {
        InitializeComponent();
        Closing += OnWindowClosing;
    }

    private async void OnWindowClosing(object? sender, WindowClosingEventArgs e)
    {
        if (DataContext is MainShellViewModel viewModel && viewModel.ProjectViewModel.IsDirty)
        {
            // Cancel the close to show the dialog
            e.Cancel = true;

            var result = await MessageBoxManager.GetMessageBoxStandard(
                "Unsaved Changes",
                "Do you want to save your changes before closing?",
                ButtonEnum.YesNoCancel,
                MsBox.Avalonia.Enums.Icon.Question
            ).ShowWindowDialogAsync(this);

            switch (result)
            {
                case ButtonResult.Yes:
                    // Save and close
                    if (viewModel.ProjectViewModel.ProjectFilePath != null)
                    {
                        await viewModel.ProjectViewModel.SaveProjectAsync();
                        CloseWithoutPrompt();
                    }
                    else
                    {
                        // Need to show save dialog using StorageProvider API
                        var file = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
                        {
                            Title = "Save Project",
                            DefaultExtension = "nisproj",
                            FileTypeChoices = new[]
                            {
                                new FilePickerFileType("NIS Project") { Patterns = new[] { "*.nisproj" } }
                            }
                        });

                        if (file != null)
                        {
                            await viewModel.ProjectViewModel.SaveProjectAsync(file.Path.LocalPath);
                            CloseWithoutPrompt();
                        }
                        // If user cancelled save dialog, don't close
                    }
                    break;

                case ButtonResult.No:
                    // Close without saving
                    CloseWithoutPrompt();
                    break;

                case ButtonResult.Cancel:
                default:
                    // Don't close, already cancelled
                    break;
            }
        }
    }

    private void CloseWithoutPrompt()
    {
        Closing -= OnWindowClosing;
        Close();
    }
}
