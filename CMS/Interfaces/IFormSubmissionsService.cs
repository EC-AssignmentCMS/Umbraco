using CMS.ViewModels;

namespace CMS.Interfaces
{
    public interface IFormSubmissionsService
    {
        Task<bool> SaveCallbackRequest(CallbackFormViewModel model);
        Task<bool> SaveNewsletterRequest(NewsletterViewModel model);
        Task<bool> SaveQuestionRequest(QuestionFormViewModel model);
    }
}