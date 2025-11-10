using CMS.Interfaces;
using CMS.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.CreateUmbracoBuilder()
    .AddBackOffice()
    .AddWebsite()
    .AddComposers()
    .Build();

builder.Services.AddScoped<IFormSubmissionsService, FormSubmissionsService>();
builder.Services.AddScoped<IEmailSender, EmailSender>();

builder.Services.AddHttpClient("EmailServiceProvider", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["EmailServiceApi"]!);
});


WebApplication app = builder.Build();

await app.BootUmbracoAsync();

app.UseHttpsRedirection();

app.UseUmbraco()
    .WithMiddleware(u =>
    {
        u.UseBackOffice();
        u.UseWebsite();
    })
    .WithEndpoints(u =>
    {
        u.UseBackOfficeEndpoints();
        u.UseWebsiteEndpoints();
    });

await app.RunAsync();
