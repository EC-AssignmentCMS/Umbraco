using CMS.ViewModels;

namespace CMS.Interfaces
{
    public interface IFormSubmissionsService
    {
        bool SaveCallbackRequest(CallbackFormViewModel model);
        bool SaveNewsletterRequest(NewsletterViewModel model);
        bool SaveQuestionRequest(QuestionFormViewModel model);
    }
}