using CMS.Models;

namespace CMS.Interfaces;

public interface IEmailSender
{
    Task SendConfirmationAsync(EmailRequestModel payload);
}