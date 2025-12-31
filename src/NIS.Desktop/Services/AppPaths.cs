using System;
using System.IO;

namespace NIS.Desktop.Services;

/// <summary>
/// Centralized path management.
/// Persistent user data is stored under %APPDATA%\SwissNISCalculator.
/// Bundled seed data is read from the app install folder.
/// </summary>
public static class AppPaths
{
    private static readonly string _appRoot = Path.GetDirectoryName(
        System.Reflection.Assembly.GetExecutingAssembly().Location) ?? ".";
    private static readonly string _appDataRoot = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "SwissNISCalculator");

    /// <summary>
    /// Application root folder (where the executable is located).
    /// </summary>
    public static string AppRoot => _appRoot;

    /// <summary>
    /// Application data root in %APPDATA%.
    /// </summary>
    public static string AppDataRoot => _appDataRoot;

    /// <summary>
    /// User settings file.
    /// </summary>
    public static string SettingsFile => Path.Combine(DataFolder, "settings.json");

    /// <summary>
    /// Data folder containing database, translations, and exports.
    /// </summary>
    public static string DataFolder => Path.Combine(AppDataRoot, "Data");

    /// <summary>
    /// Seed data folder containing the immutable seed database.
    /// </summary>
    public static string SeedDataFolder => Path.Combine(AppRoot, "Data", "seed");

    /// <summary>
    /// Bundled master data folder (antennas.json, cables.json, radios.json).
    /// </summary>
    public static string BundledDataFolder => Path.Combine(AppRoot, "Data");

    /// <summary>
    /// SQLite database file.
    /// </summary>
    public static string DatabaseFile => Path.Combine(DataFolder, "nisdata.db");

    /// <summary>
    /// Seed SQLite database file.
    /// </summary>
    public static string SeedDatabaseFile => Path.Combine(SeedDataFolder, "factory.db");

    /// <summary>
    /// Custom translations file.
    /// </summary>
    public static string TranslationsFile => Path.Combine(DataFolder, "translations.json");

    /// <summary>
    /// Master data JSON file (bands and constants).
    /// </summary>
    public static string MasterDataFile => Path.Combine(DataFolder, "masterdata.json");

    /// <summary>
    /// Default folder for projects.
    /// </summary>
    public static string ProjectsFolder => Path.Combine(DataFolder, "Projects");

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
        Directory.CreateDirectory(AppDataRoot);
        Directory.CreateDirectory(DataFolder);
        Directory.CreateDirectory(SeedDataFolder);
        Directory.CreateDirectory(ProjectsFolder);
        Directory.CreateDirectory(ExportFolder);
    }

    /// <summary>
    /// Ensures the writable database exists by copying the seed database once.
    /// </summary>
    public static void EnsureDatabaseSeeded()
    {
        if (!File.Exists(DatabaseFile))
        {
            Directory.CreateDirectory(DataFolder);
            using var _ = File.Create(DatabaseFile);
        }
    }
}
