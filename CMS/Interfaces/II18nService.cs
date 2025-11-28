namespace CMS.Interfaces;

public interface II18nService
{
    string Translate(string key, string? language = null);
    string GetCurrentLanguage();
}
