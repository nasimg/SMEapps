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
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7187/");
});

await builder.Build().RunAsync();
