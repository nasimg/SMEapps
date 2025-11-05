using Anudan.Web.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.FluentUI.AspNetCore.Components;
using SMEapps.Shared.Services;
using SMEapps.Web.Client.Services;
using SMEapps.Web.Client.Services.SStore;
using SMEapps.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddSingleton<IFormFactor, FormFactor>();
builder.Services.AddScoped<ISStore, SStore>();

var apiBaseUrl = builder.Configuration["ApiBaseUrl"];

builder.Services.AddFluentUIComponents();

// Register AuthHeader handler so it can be added to HttpClient pipeline
builder.Services.AddTransient<AuthHeader>();

// Register HttpClient with base API URL and default headers. Add handler to main ApiClient
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl ?? "https://localhost:7187/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
}).AddHttpMessageHandler<AuthHeader>();

// Add a refresh client that does NOT have the AuthHeader to avoid recursion during refresh
builder.Services.AddHttpClient("ApiClient.Refresh", client =>
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

//builder.Services.AddTransient<AuthHeaderHandler>();
// Register Authentication Services
builder.Services.AddScoped<WebAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<WebAuthStateProvider>());
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

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
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(
        typeof(SMEapps.Shared._Imports).Assembly,
        typeof(SMEapps.Web.Client._Imports).Assembly);

app.Run();
