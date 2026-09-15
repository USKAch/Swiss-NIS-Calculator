using System.Reflection;

namespace NIS.Desktop.Services;

/// <summary>
/// Application metadata derived from the assembly (version comes from &lt;Version&gt; in the csproj).
/// </summary>
public static class AppInfo
{
    /// <summary>
    /// Version string as set in the csproj / CI (e.g. "0.9.0"), without build metadata.
    /// </summary>
    public static string Version { get; } = GetVersion();

    /// <summary>
    /// Display string for footers, e.g. "Version 0.9.0".
    /// </summary>
    public static string VersionDisplay => $"Version {Version}";

    /// <summary>
    /// True when the app was started with the "--factory" switch. Factory mode
    /// (maintenance of the shipped master data, see FSD section 9) is only
    /// reachable this way; there is no password.
    /// </summary>
    public static bool FactoryModeEnabled { get; private set; }

    public static void ParseCommandLine(string[] args)
    {
        FactoryModeEnabled = System.Array.Exists(args, arg =>
            string.Equals(arg, "--factory", System.StringComparison.OrdinalIgnoreCase) ||
            string.Equals(arg, "/factory", System.StringComparison.OrdinalIgnoreCase));
    }

    private static string GetVersion()
    {
        var assembly = Assembly.GetEntryAssembly() ?? typeof(AppInfo).Assembly;
        var informational = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
        if (!string.IsNullOrWhiteSpace(informational))
        {
            // Strip source-link build metadata ("0.9.0+abc123")
            var plus = informational.IndexOf('+');
            return plus > 0 ? informational[..plus] : informational;
        }

        var version = assembly.GetName().Version;
        return version != null ? $"{version.Major}.{version.Minor}.{version.Build}" : "0.0.0";
    }
}
