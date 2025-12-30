using System.IO;

namespace NIS.Desktop.Services;

/// <summary>
/// Centralized path management for portable app.
/// All paths are relative to the executable location.
/// </summary>
public static class AppPaths
{
    private static readonly string _appRoot = Path.GetDirectoryName(
        System.Reflection.Assembly.GetExecutingAssembly().Location) ?? ".";

    /// <summary>
    /// Application root folder (where the executable is located).
    /// </summary>
    public static string AppRoot => _appRoot;

    /// <summary>
    /// User settings file.
    /// </summary>
    public static string SettingsFile => Path.Combine(DataFolder, "settings.json");

    /// <summary>
    /// Data folder containing database, translations, and exports.
    /// </summary>
    public static string DataFolder => Path.Combine(AppRoot, "Data");

    /// <summary>
    /// Seed data folder containing the immutable seed database.
    /// </summary>
    public static string SeedDataFolder => Path.Combine(DataFolder, "seed");

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
    /// Default folder for projects.
    /// </summary>
    public static string ProjectsFolder => Path.Combine(AppRoot, "Projects");

    /// <summary>
    /// Default folder for exported files (PDFs, etc.).
    /// </summary>
    public static string ExportFolder => Path.Combine(AppRoot, "Export");

    /// <summary>
    /// Ensures all required folders exist.
    /// Call this at app startup.
    /// </summary>
    public static void EnsureFoldersExist()
    {
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
        if (File.Exists(DatabaseFile))
        {
            return;
        }

        if (!File.Exists(SeedDatabaseFile))
        {
            throw new FileNotFoundException(
                $"Seed database not found at {SeedDatabaseFile}. Ensure it is shipped with the app.");
        }

        File.Copy(SeedDatabaseFile, DatabaseFile);
    }
}
