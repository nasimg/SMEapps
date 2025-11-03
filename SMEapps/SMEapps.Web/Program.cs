using Microsoft.FluentUI.AspNetCore.Components;
using SMEapps.Shared.Services;
using SMEapps.Web.Client.Services;
using SMEapps.Web.Client.Services.SStore;
using SMEapps.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

var apiBaseUrl = builder.Configuration["ApiBaseUrl"];

builder.Services.AddFluentUIComponents();

// Register HttpClient with base API URL and default headers
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl ?? "https://localhost:7187/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Dependency injection registrations
builder.Services.AddSingleton<IFormFactor, FormFactor>();
builder.Services.AddScoped<ISStore, SStore>();
builder.Services.AddTransient<AuthHeaderHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseCors("AllowAll");
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(
        typeof(SMEapps.Shared._Imports).Assembly,
        typeof(SMEapps.Web.Client._Imports).Assembly);

app.Run();
