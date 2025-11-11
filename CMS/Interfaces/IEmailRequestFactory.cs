using CMS.Models;

namespace CMS.Interfaces;

public interface IEmailRequestFactory
{
    EmailRequestModel Create<TViewModel>(TViewModel model) where TViewModel : class;
}