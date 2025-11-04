using CMS.ViewModels;
using Umbraco.Cms.Core.Services;

namespace CMS.Services;

public class FormSubmissionsService(IContentService contentService)
{
    private readonly IContentService _contentService = contentService;

    public bool SaveCallbackRequest(CallbackFormViewModel model)
    {
        try
        {
            var container = _contentService.GetRootContent().FirstOrDefault(x => x.ContentType.Alias == "formSubmissions");
            if (container == null)
                return false;

            var requestName = $"{DateTime.Now:yyyy-MM-dd HH:mm} - {model.Name}";
            var request = _contentService.Create(requestName, container, "callbackRequest");

            request.SetValue("callbackRequestName", model.Name);
            request.SetValue("callbackRequestEmail", model.Email);
            request.SetValue("callbackRequestPhone", model.Phone);
            request.SetValue("callbackRequestOption", model.SelectedOption);

            var saveResult = _contentService.Save(request);
            return saveResult.Success;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool SaveQuestionRequest(QuestionFormViewModel model)
    {
        try
        {
            var container = _contentService.GetRootContent().FirstOrDefault(x => x.ContentType.Alias == "formSubmissions");
            if (container == null)
                return false;

            var requestName = $"{DateTime.Now:yyyy-MM-dd HH:mm} - {model.Name}";
            var request = _contentService.Create(requestName, container, "questionRequest");

            request.SetValue("questionRequestName", model.Name);
            request.SetValue("questionRequestEmail", model.Email);
            request.SetValue("callbackRequestPhone", model.Question);

            var saveResult = _contentService.Save(request);
            return saveResult.Success;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool SaveNewsletterRequest(NewsletterViewModel model)
    {
        try
        {
            var container = _contentService.GetRootContent().FirstOrDefault(x => x.ContentType.Alias == "formSubmissions");
            if (container == null)
                return false;

            var requestName = $"{DateTime.Now:yyyy-MM-dd HH:mm} - Newsletter request";
            var request = _contentService.Create(requestName, container, "newsletterRequest");

            request.SetValue("newsletterRequestEmail", model.Email);

            var saveResult = _contentService.Save(request);
            return saveResult.Success;
        }
        catch (Exception)
        {
            return false;
        }
    }
}