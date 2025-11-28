using System.Text.Json;
using CMS.Interfaces;

namespace CMS.Services;

public class I18nService : II18nService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _env;
    private readonly Dictionary<string, Dictionary<string, JsonElement>> _translations = new();
    private const string DefaultLanguage = "sv";
    private const string LanguageCookieName = "lang";

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

        foreach (var file in Directory.GetFiles(i18nPath, "*.json"))
        {
            var lang = Path.GetFileNameWithoutExtension(file);
            var json = File.ReadAllText(file);
            var doc = JsonDocument.Parse(json);
            var flatDict = new Dictionary<string, JsonElement>();
            FlattenJson(doc.RootElement, "", flatDict);
            _translations[lang] = flatDict;
        }
    }

    private void FlattenJson(JsonElement element, string prefix, Dictionary<string, JsonElement> dict)
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
            default:
                dict[prefix] = element;
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
            _translations.ContainsKey(cookieLang))
        {
            return cookieLang;
        }

        // Check query string
        if (context.Request.Query.TryGetValue("lang", out var queryLang) &&
            !string.IsNullOrEmpty(queryLang) &&
            _translations.ContainsKey(queryLang!))
        {
            return queryLang!;
        }

        return DefaultLanguage;
    }

    public string Translate(string key, string? language = null)
    {
        var lang = language ?? GetCurrentLanguage();

        if (_translations.TryGetValue(lang, out var langDict) &&
            langDict.TryGetValue(key, out var value))
        {
            return value.GetString() ?? key;
        }

        // Fallback to default language
        if (lang != DefaultLanguage &&
            _translations.TryGetValue(DefaultLanguage, out var defaultDict) &&
            defaultDict.TryGetValue(key, out var defaultValue))
        {
            return defaultValue.GetString() ?? key;
        }

        return key;
    }
}
