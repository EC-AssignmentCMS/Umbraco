using CMS.Services;
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
    FormSubmissionsService formSubmissions,
    IUmbracoContextAccessor umbracoContextAccessor,
    IUmbracoDatabaseFactory databaseFactory,
    ServiceContext services,
    AppCaches appCaches,
    IProfilingLogger profilingLogger,
    IPublishedUrlProvider publishedUrlProvider
    ) : SurfaceController(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
{
    private readonly FormSubmissionsService _formSubmissions = formSubmissions;

    public async Task<IActionResult> HandleCallbackForm(CallbackFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return CurrentUmbracoPage();
        }

        var result = await _formSubmissions.SaveCallbackRequest(model);
        if (!result)
        {
            TempData["FormError"] = "Something went wrong while submitting your request. Please try again later.";
            return RedirectToCurrentUmbracoPage();
        }

        TempData["FormSuccess"] = "Thank you! Your request has been received and we will get back to you soon.";

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
            TempData["FormError"] = "Something went wrong while submitting your request. Please try again later.";
            return RedirectToCurrentUmbracoPage();
        }

        TempData["FormSuccess"] = "Thank you! Your request has been received and we will get back to you soon.";

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
            TempData["SignUpError"] = "Error. Please try again later.";
            return RedirectToCurrentUmbracoPage();
        }

        TempData["SignUpSuccess"] = "Thank you for signing up!";

        return RedirectToCurrentUmbracoPage();
    }
}