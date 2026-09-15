using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NIS.Desktop.Localization;
using NIS.Desktop.Services;
using Xunit;

namespace NIS.Desktop.Tests;

/// <summary>
/// Guards the localization table: every string property must have a translation
/// in all four languages, and every translation entry must be reachable via a property.
/// </summary>
public class LocalizationTests
{
    private static readonly string[] Languages = { "de", "fr", "it", "en" };

    private static Dictionary<string, Dictionary<string, string>> TranslationData =>
        (Dictionary<string, Dictionary<string, string>>)typeof(Strings)
            .GetField("TranslationData", BindingFlags.NonPublic | BindingFlags.Static)!
            .GetValue(null)!;

    // Properties that are computed rather than looked up
    private static readonly HashSet<string> ComputedProperties = new()
    {
        nameof(Strings.Instance), nameof(Strings.Language),
        nameof(Strings.UnitMeter), nameof(Strings.UnitMhz),
    };

    private static IEnumerable<string> StringPropertyNames() =>
        typeof(Strings).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.PropertyType == typeof(string) && !ComputedProperties.Contains(p.Name))
            .Select(p => p.Name);

    [Fact]
    public void EveryTranslationEntry_HasAllFourLanguages()
    {
        var incomplete = TranslationData
            .Where(kv => Languages.Any(l => !kv.Value.ContainsKey(l) || string.IsNullOrWhiteSpace(kv.Value[l])))
            .Select(kv => kv.Key)
            .ToList();

        Assert.True(incomplete.Count == 0, "Missing translations for: " + string.Join(", ", incomplete));
    }

    [Fact]
    public void NoStringProperty_ResolvesToMissingKeyMarker()
    {
        var strings = Strings.Instance;
        var missing = new List<string>();

        foreach (var lang in Languages)
        {
            strings.Language = lang;
            foreach (var name in StringPropertyNames())
            {
                var value = (string?)typeof(Strings).GetProperty(name)!.GetValue(strings);
                // Get() returns "[Key]" for a missing translation
                if (value == null || value == $"[{name}]")
                    missing.Add($"{name} ({lang})");
            }
        }

        strings.Language = "de";
        Assert.True(missing.Count == 0, "Properties without translation: " + string.Join(", ", missing));
    }

    [Fact]
    public void DefaultTranslation_SurvivesRuntimeOverride()
    {
        var original = Strings.GetDefaultTranslation("Save", "de");
        Assert.Equal("Speichern", original);

        Strings.UpdateTranslation("Save", "de", "Sichern");
        try
        {
            Assert.Equal("Sichern", Strings.Instance.Get("Save"));
            Assert.Equal("Speichern", Strings.GetDefaultTranslation("Save", "de"));
            Assert.False(Strings.IsDefaultTranslation("Save", "de", "Sichern"));
            Assert.True(Strings.IsDefaultTranslation("Save", "de", "Speichern"));
            Assert.False(Strings.IsDefaultTranslation("NoSuchKey", "de", "x"));
        }
        finally
        {
            Strings.UpdateTranslation("Save", "de", original!);
        }
    }

    [Fact]
    public void Get_UnknownKey_ReturnsBracketedMarker()
    {
        Assert.Equal("[DoesNotExist]", Strings.Instance.Get("DoesNotExist"));
    }

    [Fact]
    public void Get_FallsBackToGermanForUnknownLanguage()
    {
        var strings = Strings.Instance;
        strings.Language = "xx";
        Assert.Equal(TranslationData["Save"]["de"], strings.Save);
        strings.Language = "de";
    }

    [Fact]
    public void SettingsAboutVersion_ContainsAssemblyVersion()
    {
        Assert.Contains(AppInfo.Version, Strings.Instance.SettingsAboutVersion);
    }

    [Fact]
    public void ChangingLanguage_RaisesPropertyChanged()
    {
        var strings = Strings.Instance;
        var raised = new List<string>();
        void Handler(object? s, System.ComponentModel.PropertyChangedEventArgs e) => raised.Add(e.PropertyName ?? "");
        strings.PropertyChanged += Handler;
        try
        {
            strings.Language = "fr";
            Assert.Contains(nameof(Strings.Save), raised);
            Assert.Contains(nameof(Strings.Band), raised);
            Assert.Contains(nameof(Strings.ParcelNumber), raised);
            Assert.Contains(nameof(Strings.LanguageSystem), raised);
        }
        finally
        {
            strings.PropertyChanged -= Handler;
            strings.Language = "de";
        }
    }
}
