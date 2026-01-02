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
    private static readonly string _appRoot = Path.GetDirectoryName(
        System.Reflection.Assembly.GetExecutingAssembly().Location) ?? ".";

    /// <summary>
    /// Application root folder (where the executable is located).
    /// </summary>
    public static string AppRoot => _appRoot;

    /// <summary>
    /// Data folder containing database, settings, translations, and exports.
    /// Located in the application folder for portability.
    /// </summary>
    public static string DataFolder => Path.Combine(AppRoot, "Data");

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
}
