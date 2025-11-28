using CMS.Interfaces;
using CMS.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

namespace CMS.Controllers;

public class FormController(
    IFormSubmissionsService formSubmissions,
    II18nService i18nService,
    IUmbracoContextAccessor umbracoContextAccessor,
    IUmbracoDatabaseFactory databaseFactory,
    ServiceContext services,
    AppCaches appCaches,
    IProfilingLogger profilingLogger,
    IPublishedUrlProvider publishedUrlProvider
    ) : SurfaceController(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
{
    private readonly IFormSubmissionsService _formSubmissions = formSubmissions;
    private readonly II18nService _i18n = i18nService;

    public async Task<IActionResult> HandleCallbackForm(CallbackFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return CurrentUmbracoPage();
        }

        var result = await _formSubmissions.SaveCallbackRequest(model);
        if (!result)
        {
            TempData["FormError"] = _i18n.Translate("forms.messages.formError");
            return RedirectToCurrentUmbracoPage();
        }

        TempData["FormSuccess"] = _i18n.Translate("forms.messages.formSuccess");

        return RedirectToCurrentUmbracoPage();
    }

    public async Task<IActionResult> HandleQuestionForm(QuestionFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return CurrentUmbracoPage();
        }

        var result = await _formSubmissions.SaveQuestionRequest(model);
        if (!result)
        {
            TempData["FormError"] = _i18n.Translate("forms.messages.formError");
            return RedirectToCurrentUmbracoPage();
        }

        TempData["FormSuccess"] = _i18n.Translate("forms.messages.formSuccess");

        return RedirectToCurrentUmbracoPage();
    }


    public async Task<IActionResult> HandleNewsletterForm(NewsletterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return CurrentUmbracoPage();
        }

        var result = await _formSubmissions.SaveNewsletterRequest(model);
        if (!result)
        {
            TempData["SignUpError"] = _i18n.Translate("forms.messages.signUpError");
            return RedirectToCurrentUmbracoPage();
        }

        TempData["SignUpSuccess"] = _i18n.Translate("forms.messages.signUpSuccess");

        return RedirectToCurrentUmbracoPage();
    }
}