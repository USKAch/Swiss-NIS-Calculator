using System;
using System.IO;

namespace NIS.Desktop.Services;

/// <summary>
/// Centralized path management.
/// Portable application: all data is stored in the application folder.
/// No data is stored in %APPDATA% or other system folders.
/// </summary>
public static class AppPaths
{
    private const string AppFolderName = "SwissNISCalculator";

    private static readonly string _appRoot = AppContext.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar);
    private static readonly string _portableDataFolder = Path.Combine(AppRoot, "Data");
    private static bool _usingPortableStorage;
    private static readonly string _dataFolder = InitializeDataFolder();

    /// <summary>
    /// Application root folder (where the executable is located).
    /// </summary>
    public static string AppRoot => _appRoot;

    /// <summary>
    /// Indicates whether we are storing data next to the executable (portable mode).
    /// When false, data lives in the per-user application support directory.
    /// </summary>
    public static bool UsingPortableStorage => _usingPortableStorage;

    /// <summary>
    /// Data folder containing database, settings, translations, and exports.
    /// </summary>
    public static string DataFolder => _dataFolder;

    /// <summary>
    /// User settings file.
    /// </summary>
    public static string SettingsFile => Path.Combine(DataFolder, "settings.json");

    /// <summary>
    /// SQLite database file containing all master data and projects.
    /// </summary>
    public static string DatabaseFile => Path.Combine(DataFolder, "nisdata.db");

    /// <summary>
    /// Custom translations file.
    /// </summary>
    public static string TranslationsFile => Path.Combine(DataFolder, "translations.json");

    /// <summary>
    /// Master data JSON file (bands and constants) - for import/export only.
    /// </summary>
    public static string MasterDataFile => Path.Combine(DataFolder, "masterdata.json");

    /// <summary>
    /// Default folder for exported files (PDFs, etc.).
    /// </summary>
    public static string ExportFolder => Path.Combine(DataFolder, "Export");

    /// <summary>
    /// Ensures all required folders exist.
    /// Call this at app startup.
    /// </summary>
    public static void EnsureFoldersExist()
    {
        Directory.CreateDirectory(DataFolder);
        Directory.CreateDirectory(ExportFolder);
    }

    private static string InitializeDataFolder()
    {
        if (TryPreparePortableStorage(_portableDataFolder))
        {
            _usingPortableStorage = true;
            return _portableDataFolder;
        }

        var fallback = GetFallbackDataFolder();
        CopyDataIfMissing(_portableDataFolder, fallback);
        _usingPortableStorage = false;
        return fallback;
    }

    private static bool TryPreparePortableStorage(string dataFolder)
    {
        try
        {
            Directory.CreateDirectory(dataFolder);
            var exportFolder = Path.Combine(dataFolder, "Export");
            Directory.CreateDirectory(exportFolder);

            var testFile = Path.Combine(dataFolder, ".write_test");
            using var stream = File.Create(testFile, 1, FileOptions.DeleteOnClose);

            return true;
        }
        catch
        {
            return false;
        }
    }

    private static string GetFallbackDataFolder()
    {
        var basePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        if (string.IsNullOrWhiteSpace(basePath))
        {
            basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        }
        if (string.IsNullOrWhiteSpace(basePath))
        {
            basePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        }
        if (string.IsNullOrWhiteSpace(basePath))
        {
            basePath = Path.GetTempPath();
        }

        var fallbackRoot = Path.Combine(basePath, AppFolderName);
        var fallbackData = Path.Combine(fallbackRoot, "Data");
        Directory.CreateDirectory(fallbackData);
        Directory.CreateDirectory(Path.Combine(fallbackData, "Export"));
        return fallbackData;
    }

    private static void CopyDataIfMissing(string source, string destination)
    {
        if (!Directory.Exists(source))
        {
            return;
        }

        foreach (var file in Directory.GetFiles(source, "*", SearchOption.AllDirectories))
        {
            var relativePath = Path.GetRelativePath(source, file);
            var targetFile = Path.Combine(destination, relativePath);
            if (!File.Exists(targetFile))
            {
                var targetDir = Path.GetDirectoryName(targetFile);
                if (!string.IsNullOrEmpty(targetDir))
                {
                    Directory.CreateDirectory(targetDir);
                }
                File.Copy(file, targetFile);
                TryMakeWritable(targetFile);
            }
        }
    }

    private static void TryMakeWritable(string path)
    {
        try
        {
            File.SetAttributes(path, FileAttributes.Normal);
        }
        catch
        {
        }

#if NET6_0_OR_GREATER
        if (!OperatingSystem.IsWindows())
        {
            try
            {
                const UnixFileMode mode = UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.GroupRead;
                File.SetUnixFileMode(path, mode);
            }
            catch
            {
            }
        }
#endif
    }
}
