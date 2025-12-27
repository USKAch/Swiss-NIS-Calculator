using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;

namespace NIS.Desktop.New.Services;

/// <summary>
/// Provides localized strings from embedded JSON resources.
/// </summary>
public partial class LocalizationService : ObservableObject, ILocalizationService
{
    private Dictionary<string, string> _strings = new();

    [ObservableProperty]
    private string _currentLanguage = "de";

    public string[] AvailableLanguages => new[] { "de", "en", "fr", "it" };

    public event Action? LanguageChanged;

    public LocalizationService()
    {
        LoadStrings("de");
    }

    public string this[string key]
    {
        get
        {
            if (_strings.TryGetValue(key, out var value))
            {
                return value;
            }
            // Return key as fallback (helps identify missing translations)
            return $"[{key}]";
        }
    }

    public string GetString(string key, params object[] args)
    {
        var format = this[key];
        if (args.Length > 0)
        {
            try
            {
                return string.Format(format, args);
            }
            catch
            {
                return format;
            }
        }
        return format;
    }

    partial void OnCurrentLanguageChanged(string value)
    {
        LoadStrings(value);
        LanguageChanged?.Invoke();

        // Notify that all indexed properties have changed (for binding refresh)
        OnPropertyChanged("Item[]");
    }

    private void LoadStrings(string language)
    {
        _strings = new Dictionary<string, string>();

        try
        {
            // Try to load from embedded resource
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"NIS.Desktop.New.Resources.Strings.strings.{language}.json";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream != null)
            {
                using var reader = new StreamReader(stream);
                var json = reader.ReadToEnd();
                var data = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                if (data != null)
                {
                    _strings = data;
                }
            }
            else
            {
                // Fallback: try to load from file system (for development)
                var basePath = AppContext.BaseDirectory;
                var filePath = Path.Combine(basePath, "Resources", "Strings", $"strings.{language}.json");

                if (File.Exists(filePath))
                {
                    var json = File.ReadAllText(filePath);
                    var data = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                    if (data != null)
                    {
                        _strings = data;
                    }
                }
            }
        }
        catch
        {
            // If loading fails, use empty dictionary (keys will be returned as-is)
        }
    }
}
