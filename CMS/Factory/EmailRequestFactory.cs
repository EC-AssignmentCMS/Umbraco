using CMS.Interfaces;
using CMS.Models;
using CMS.ViewModels;

namespace CMS.Factory;

public class EmailRequestFactory : IEmailRequestFactory
{
    public EmailRequestModel Create<TViewModel>(TViewModel model) where TViewModel : class
    {
        return model switch
        {
            CallbackFormViewModel m => new EmailRequestModel
            {
                FormType = nameof(CallbackFormViewModel),
                Name = m.Name,
                Email = m.Email,
                Phone = m.Phone,
                SelectedOption = m.SelectedOption
            },
            QuestionFormViewModel m => new EmailRequestModel
            {
                FormType = nameof(QuestionFormViewModel),
                Name = m.Name,
                Email = m.Email,
                Question = m.Question
            },
            NewsletterViewModel m => new EmailRequestModel
            {
                FormType = nameof(NewsletterViewModel),
                Email = m.NewsletterEmail
            },
            _ => throw new ArgumentException($"Unsupported form type: {typeof(TViewModel).Name}")
        };
    }
}
