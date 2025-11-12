using Anudan.Web.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using SMEapps.Shared.Services;
using SMEapps.Web.Client.Services;
using SMEapps.Web.Client.Services.SStore;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// ?? UI Framework
builder.Services.AddMudServices();

// ?? Device-specific services
builder.Services.AddSingleton<IFormFactor, FormFactor>();
builder.Services.AddSingleton<ISStore, SStore>();

// ?? Dashboard and common code services
builder.Services.AddScoped<DashboardService>();

// ? Register IHttpClientFactory support (needed for your CommonCodeService)
builder.Services.AddHttpClient();

// ? Register CommonCodeService so it can be injected in components
builder.Services.AddScoped<CommonCodeService>();

// ?? Auth handler and HTTP clients
builder.Services.AddTransient<AuthHeader>();

builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7187/");
})
.AddHttpMessageHandler<AuthHeader>();

builder.Services.AddHttpClient("ApiClient.Refresh", client =>
{
    client.BaseAddress = new Uri("https://localhost:7187/");
});

// ?? Authentication & authorization
builder.Services.AddScoped<WebAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<WebAuthStateProvider>());
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
