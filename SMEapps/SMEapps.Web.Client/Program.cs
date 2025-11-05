using Anudan.Web.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
using SMEapps.Shared.Services;
using SMEapps.Web.Client.Services;
using SMEapps.Web.Client.Services.SStore;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddFluentUIComponents();

// Add device-specific services used by the SMEapps.Shared project
builder.Services.AddSingleton<IFormFactor, FormFactor>();
builder.Services.AddSingleton<ISStore, SStore>();

// Register AuthHeader handler so it can be added to HttpClient pipeline
builder.Services.AddTransient<AuthHeader>();

// Add HttpClient for API calls — main client has the AuthHeader
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7187/");
})
.AddHttpMessageHandler<AuthHeader>();

// Add a second client used for token refresh that does NOT use the AuthHeader (avoids recursion)
builder.Services.AddHttpClient("ApiClient.Refresh", client =>
{
    client.BaseAddress = new Uri("https://localhost:7187/");
});

// Register Authentication Services
builder.Services.AddScoped<WebAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<WebAuthStateProvider>());
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
