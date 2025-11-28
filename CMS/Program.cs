using CMS.Factory;
using CMS.Interfaces;
using CMS.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.CreateUmbracoBuilder()
    .AddBackOffice()
    .AddWebsite()
    .AddComposers()
    .Build();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<II18nService, I18nService>();
builder.Services.AddScoped<IFormSubmissionsService, FormSubmissionsService>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IEmailRequestFactory, EmailRequestFactory>();

builder.Services.AddHttpClient("EmailServiceProvider", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["EmailServiceApi"]!);
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
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
