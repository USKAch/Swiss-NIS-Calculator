using Avalonia;
using System;
using NIS.Desktop.Services;

namespace NIS.Desktop;

class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        // Ensure portable app folders exist before anything else
        AppPaths.EnsureFoldersExist();

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
