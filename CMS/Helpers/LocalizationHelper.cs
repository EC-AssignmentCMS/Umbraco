using CMS.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CMS.Helpers;

public static class LocalizationHelper
{
    public static string T(this IHtmlHelper html, string key)
    {
        var httpContext = html.ViewContext.HttpContext;
        var i18nService = httpContext.RequestServices.GetService<II18nService>();
        return i18nService?.Translate(key) ?? key;
    }

    public static string CurrentLang(this IHtmlHelper html)
    {
        var httpContext = html.ViewContext.HttpContext;
        var i18nService = httpContext.RequestServices.GetService<II18nService>();
        return i18nService?.GetCurrentLanguage() ?? "sv";
    }
}
