using Anudan.Shared.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;
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

            // Load appsettings.json
            builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);


            var apiBaseUrl = builder.Configuration["ApiBaseUrl"];

            builder.Services.AddHttpClient("ApiClient", client =>
            {
                client.BaseAddress = new Uri(apiBaseUrl ?? "http://192.168.1.38:9095/");
            }).AddHttpMessageHandler<AuthHeader>();
            builder.Services.AddTransient<AuthHeader>();

            builder.Services.AddScoped<MauiAuthStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider, MauiAuthStateProvider>();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddSingleton<ISecureStorage>(SecureStorage.Default);
            builder.Services.AddSingleton<IFormFactor, FormFactor>();
            builder.Services.AddSingleton<ISStore, SStore>();

            builder.Services.AddTransient<AuthHeader>();
            builder.Services.AddScoped<DashboardService>();
            builder.Services.AddScoped<CustomerService>();
            builder.Services.AddScoped<CommonCodeService>();
            builder.Services.AddScoped<ModuleService>();
            builder.Services.AddScoped<ConfirmDialogService>();
            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddMudServices();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
