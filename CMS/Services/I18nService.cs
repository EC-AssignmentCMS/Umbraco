using System.Text.Json;
using CMS.Interfaces;

namespace CMS.Services;

public class I18nService : II18nService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _env;
    private readonly Dictionary<string, Dictionary<string, string>> _translations = new();
    private const string DefaultLanguage = "sv";
    private const string LanguageCookieName = "lang";
    private static readonly string[] SupportedLanguages = ["sv", "en"];

    public I18nService(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env)
    {
        _httpContextAccessor = httpContextAccessor;
        _env = env;
        LoadTranslations();
    }

    private void LoadTranslations()
    {
        var i18nPath = Path.Combine(_env.WebRootPath, "i18n");
        if (!Directory.Exists(i18nPath)) return;

        foreach (var lang in SupportedLanguages)
        {
            var filePath = Path.Combine(i18nPath, $"{lang}.json");
            if (!File.Exists(filePath)) continue;

            try
            {
                var json = File.ReadAllText(filePath);
                using var doc = JsonDocument.Parse(json);
                var flatDict = new Dictionary<string, string>();
                FlattenJson(doc.RootElement, "", flatDict);
                _translations[lang] = flatDict;
            }
            catch (Exception)
            {
                // Log error in production, but don't crash the app
                continue;
            }
        }
    }

    private void FlattenJson(JsonElement element, string prefix, Dictionary<string, string> dict)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                foreach (var prop in element.EnumerateObject())
                {
                    var newPrefix = string.IsNullOrEmpty(prefix) ? prop.Name : $"{prefix}.{prop.Name}";
                    FlattenJson(prop.Value, newPrefix, dict);
                }
                break;
            case JsonValueKind.String:
                dict[prefix] = element.GetString() ?? prefix;
                break;
            default:
                dict[prefix] = element.ToString();
                break;
        }
    }

    public string GetCurrentLanguage()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null) return DefaultLanguage;

        // Check cookie first
        if (context.Request.Cookies.TryGetValue(LanguageCookieName, out var cookieLang) &&
            !string.IsNullOrEmpty(cookieLang) &&
            IsValidLanguage(cookieLang))
        {
            return cookieLang;
        }

        // Check query string
        if (context.Request.Query.TryGetValue("lang", out var queryLang) &&
            !string.IsNullOrEmpty(queryLang) &&
            IsValidLanguage(queryLang!))
        {
            return queryLang!;
        }

        return DefaultLanguage;
    }

    private static bool IsValidLanguage(string lang)
    {
        return SupportedLanguages.Contains(lang, StringComparer.OrdinalIgnoreCase);
    }

    public string Translate(string key, string? language = null)
    {
        var lang = language ?? GetCurrentLanguage();

        if (_translations.TryGetValue(lang, out var langDict) &&
            langDict.TryGetValue(key, out var value))
        {
            return value;
        }

        // Fallback to default language
        if (lang != DefaultLanguage &&
            _translations.TryGetValue(DefaultLanguage, out var defaultDict) &&
            defaultDict.TryGetValue(key, out var defaultValue))
        {
            return defaultValue;
        }

        return key;
    }
}
