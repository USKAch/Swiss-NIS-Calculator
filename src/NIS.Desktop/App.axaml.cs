using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data;
using Avalonia.Data.Core.Plugins;
using Avalonia.Input;
using Avalonia.Logging;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Avalonia.Threading;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using NIS.Desktop.Services;
using NIS.Desktop.ViewModels;

namespace NIS.Desktop;

public partial class App : Application
{
    private MainShellViewModel? _mainShellViewModel;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

        // Debug: Log binding errors to console
        Logger.Sink = new DebugLogSink();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Load any custom translations from previous sessions
            TranslationEditorViewModel.LoadCustomTranslations();

            // Avoid duplicate validations from both Avalonia and the CommunityToolkit.
            DisableAvaloniaDataAnnotationValidation();

            _mainShellViewModel = new MainShellViewModel();

            // Set up confirmation dialog callback
            _mainShellViewModel.ShowConfirmDialog = async (title, message) =>
            {
                var msgBox = MessageBoxManager.GetMessageBoxStandard(
                    title,
                    message,
                    ButtonEnum.YesNo,
                    Icon.Question);
                var result = await msgBox.ShowAsync();
                return result == ButtonResult.Yes;
            };

            // Apply initial theme from settings
            var settings = Services.AppSettings.Load();
            SettingsViewModel.ApplyTheme(settings.ThemeMode);

            var mainWindow = new MainWindow
            {
                DataContext = _mainShellViewModel
            };

            // Add global handler for NumericUpDown to select all on focus
            mainWindow.AddHandler(InputElement.GotFocusEvent, OnNumericUpDownGotFocus, handledEventsToo: true);

            desktop.MainWindow = mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void OnNumericUpDownGotFocus(object? sender, GotFocusEventArgs e)
    {
        if (e.Source is TextBox textBox && textBox.Parent?.Parent is NumericUpDown)
        {
            // Select all text when NumericUpDown's inner TextBox gets focus
            Dispatcher.UIThread.Post(() => textBox.SelectAll());
        }
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}

/// <summary>
/// Debug log sink to trace binding errors.
/// </summary>
internal class DebugLogSink : ILogSink
{
    public bool IsEnabled(LogEventLevel level, string area) => level >= LogEventLevel.Warning;

    public void Log(LogEventLevel level, string area, object? source, string messageTemplate)
    {
        Debug.WriteLine($"[{level}] {area}: {messageTemplate}");
    }

    public void Log(LogEventLevel level, string area, object? source, string messageTemplate, params object?[] propertyValues)
    {
        var message = string.Format(messageTemplate.Replace("{", "{{").Replace("}", "}}"), propertyValues);
        Debug.WriteLine($"[{level}] {area}: {message} (Source: {source?.GetType().Name})");
    }
}
