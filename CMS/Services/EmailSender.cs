using CMS.Interfaces;

namespace CMS.Services;

public class EmailSender(IHttpClientFactory httpClientFactory) : IEmailSender
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public async Task SendConfirmationAsync(string email, string formType)
    {
        var client = _httpClientFactory.CreateClient("EmailServiceProvider");

        var payload = new { Email = email, FormType = formType };

        var resp = await client.PostAsJsonAsync("api/Confirmation/send-confirmation-email", payload);
        resp.EnsureSuccessStatusCode();
    }

}