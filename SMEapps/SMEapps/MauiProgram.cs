using Microsoft.Extensions.Logging;
using Microsoft.FluentUI.AspNetCore.Components;
using SMEapps.Services;
using SMEapps.Shared.Services;

namespace SMEapps
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            // Add device-specific services used by the SMEapps.Shared project
            builder.Services.AddSingleton<IFormFactor, FormFactor>();
            var apiBaseUrl = builder.Configuration["ApiBaseUrl"];

            builder.Services.AddHttpClient("ApiClient", client =>
            {
                client.BaseAddress = new Uri(apiBaseUrl);
            }).AddHttpMessageHandler<AuthHeader>();
            builder.Services.AddTransient<AuthHeader>();

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddFluentUIComponents();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif
            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7187/")
            });

            return builder.Build();
        }
    }
}
