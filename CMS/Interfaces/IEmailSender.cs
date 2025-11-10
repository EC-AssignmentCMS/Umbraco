namespace CMS.Interfaces
{
    public interface IEmailSender
    {
        Task SendConfirmationAsync(string email, string formType);
    }
}