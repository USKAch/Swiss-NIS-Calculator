using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NIS.Desktop.New.Localization;

namespace NIS.Desktop.New.ViewModels;

/// <summary>
/// Represents a single translation entry for editing.
/// </summary>
public partial class TranslationItem : ObservableObject
{
    [ObservableProperty]
    private string _key = string.Empty;

    [ObservableProperty]
    private string _category = string.Empty;

    [ObservableProperty]
    private string _german = string.Empty;

    [ObservableProperty]
    private string _french = string.Empty;

    [ObservableProperty]
    private string _italian = string.Empty;

    [ObservableProperty]
    private string _english = string.Empty;

    public bool IsModified { get; set; }

    partial void OnGermanChanged(string value) => IsModified = true;
    partial void OnFrenchChanged(string value) => IsModified = true;
    partial void OnItalianChanged(string value) => IsModified = true;
    partial void OnEnglishChanged(string value) => IsModified = true;
}

/// <summary>
/// ViewModel for the in-app translation editor.
/// </summary>
public partial class TranslationEditorViewModel : ViewModelBase
{
    private static readonly string TranslationsFilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "SwissNISCalculator",
        "translations.json");

    public ObservableCollection<TranslationItem> AllTranslations { get; } = new();
    public ObservableCollection<TranslationItem> FilteredTranslations { get; } = new();
    public ObservableCollection<string> Categories { get; } = new() { "All" };

    /// <summary>
    /// Callback to show confirmation dialog. Returns true if user confirms.
    /// </summary>
    public Func<string, string, Task<bool>>? ConfirmDiscardChanges { get; set; }

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private string _selectedCategory = "All";

    [ObservableProperty]
    private TranslationItem? _selectedItem;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private bool _hasUnsavedChanges;

    public TranslationEditorViewModel()
    {
        LoadTranslations();
    }

    /// <summary>
    /// Checks if there are any modified translations.
    /// </summary>
    public bool HasModifiedItems => AllTranslations.Any(t => t.IsModified);

    /// <summary>
    /// Loads translations from Strings class into editable items.
    /// </summary>
    private void LoadTranslations()
    {
        AllTranslations.Clear();
        Categories.Clear();
        Categories.Add("All");

        // Load from the Strings.Translations dictionary
        var translations = Strings.GetAllTranslations();
        var categories = new HashSet<string>();

        foreach (var (key, category, values) in translations)
        {
            var item = new TranslationItem
            {
                Key = key,
                Category = category,
                German = values.GetValueOrDefault("de", ""),
                French = values.GetValueOrDefault("fr", ""),
                Italian = values.GetValueOrDefault("it", ""),
                English = values.GetValueOrDefault("en", ""),
                IsModified = false
            };
            AllTranslations.Add(item);

            if (!string.IsNullOrEmpty(category))
            {
                categories.Add(category);
            }
        }

        // Add categories sorted
        foreach (var cat in categories.OrderBy(c => c))
        {
            Categories.Add(cat);
        }

        ApplyFilter();
        StatusMessage = $"Loaded {AllTranslations.Count} translations";
    }

    partial void OnSearchTextChanged(string value) => ApplyFilter();
    partial void OnSelectedCategoryChanged(string value) => ApplyFilter();

    private void ApplyFilter()
    {
        FilteredTranslations.Clear();

        var filtered = AllTranslations.AsEnumerable();

        // Filter by category
        if (!string.IsNullOrEmpty(SelectedCategory) && SelectedCategory != "All")
        {
            filtered = filtered.Where(t => t.Category == SelectedCategory);
        }

        // Filter by search text
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var search = SearchText.ToLowerInvariant();
            filtered = filtered.Where(t =>
                t.Key.ToLowerInvariant().Contains(search) ||
                t.German.ToLowerInvariant().Contains(search) ||
                t.French.ToLowerInvariant().Contains(search) ||
                t.Italian.ToLowerInvariant().Contains(search) ||
                t.English.ToLowerInvariant().Contains(search));
        }

        foreach (var item in filtered.OrderBy(t => t.Category).ThenBy(t => t.Key))
        {
            FilteredTranslations.Add(item);
        }
    }

    [RelayCommand]
    private void ClearSearch()
    {
        SearchText = string.Empty;
    }

    [RelayCommand]
    private void SaveTranslations()
    {
        try
        {
            // Update the Strings class with modified values
            foreach (var item in AllTranslations.Where(t => t.IsModified))
            {
                Strings.UpdateTranslation(item.Key, "de", item.German);
                Strings.UpdateTranslation(item.Key, "fr", item.French);
                Strings.UpdateTranslation(item.Key, "it", item.Italian);
                Strings.UpdateTranslation(item.Key, "en", item.English);
                item.IsModified = false;
            }

            // Save to JSON file for persistence
            SaveToFile();

            HasUnsavedChanges = false;
            StatusMessage = "Translations saved successfully. Changes take effect immediately.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error saving: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task CancelChanges()
    {
        if (HasModifiedItems)
        {
            // Ask for confirmation
            if (ConfirmDiscardChanges != null)
            {
                var confirmed = await ConfirmDiscardChanges(
                    Strings.Instance.DiscardChanges,
                    Strings.Instance.DiscardChangesPrompt);

                if (!confirmed)
                    return;
            }
        }

        // Reload translations from Strings (discards changes)
        LoadTranslations();
        HasUnsavedChanges = false;
        StatusMessage = Strings.Instance.ChangesDiscarded;
    }

    /// <summary>
    /// Saves translations to a JSON file for persistence across app restarts.
    /// </summary>
    private void SaveToFile()
    {
        var dir = Path.GetDirectoryName(TranslationsFilePath);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        var data = AllTranslations.Select(t => new Dictionary<string, string>
        {
            ["Key"] = t.Key,
            ["Category"] = t.Category,
            ["de"] = t.German,
            ["fr"] = t.French,
            ["it"] = t.Italian,
            ["en"] = t.English
        }).ToList();

        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(TranslationsFilePath, json);
    }

    /// <summary>
    /// Loads custom translations from file and merges with defaults.
    /// Called at app startup.
    /// </summary>
    public static void LoadCustomTranslations()
    {
        if (!File.Exists(TranslationsFilePath))
            return;

        try
        {
            var json = File.ReadAllText(TranslationsFilePath);
            var data = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(json);

            if (data != null)
            {
                foreach (var item in data)
                {
                    var key = item.GetValueOrDefault("Key", "");
                    if (!string.IsNullOrEmpty(key))
                    {
                        // Only update if value is non-empty to preserve defaults
                        if (item.TryGetValue("de", out var de) && !string.IsNullOrEmpty(de))
                            Strings.UpdateTranslation(key, "de", de);
                        if (item.TryGetValue("fr", out var fr) && !string.IsNullOrEmpty(fr))
                            Strings.UpdateTranslation(key, "fr", fr);
                        if (item.TryGetValue("it", out var it) && !string.IsNullOrEmpty(it))
                            Strings.UpdateTranslation(key, "it", it);
                        if (item.TryGetValue("en", out var en) && !string.IsNullOrEmpty(en))
                            Strings.UpdateTranslation(key, "en", en);
                    }
                }
            }
        }
        catch
        {
            // Ignore errors loading custom translations
        }
    }
}
