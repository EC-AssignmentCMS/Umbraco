using CMS.Interfaces;
using CMS.Models;

namespace CMS.Services;

public class EmailSender(IHttpClientFactory httpClientFactory, IConfiguration config) : IEmailSender
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly string _endpoint = config["EmailServiceEndpoints:SendConfirmation"]!;

    public async Task SendConfirmationAsync(EmailRequestModel payload)
    {
        var client = _httpClientFactory.CreateClient("EmailServiceProvider");

        var response = await client.PostAsJsonAsync(_endpoint, payload);
        response.EnsureSuccessStatusCode();
    }
}