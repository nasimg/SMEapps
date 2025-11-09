using Anudan.Web.Client.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;
using MudBlazor.Services;
using SMEapps.Shared.Services;
using SMEapps.Web.Client.Services;
using SMEapps.Web.Client.Services.SStore;
using SMEapps.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddMudServices();

builder.Services.AddSingleton<IFormFactor, FormFactor>();
builder.Services.AddScoped<ISStore, SStore>();

var apiBaseUrl = builder.Configuration["ApiBaseUrl"];

//builder.Services.AddFluentUIComponents();

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

// Add CORS — allow the Blazor WASM origin and required headers/methods
var allowedClientOrigin = builder.Configuration["ClientOrigin"] ?? "https://localhost:7206";
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClient", policy =>
    {
        policy.WithOrigins(allowedClientOrigin)
              .AllowAnyMethod()
              .AllowAnyHeader();
        // If you need to allow cookies, call .AllowCredentials() and DO NOT use WithOrigins("*")
    });
});

// Register authentication services so IAuthenticationService is available
// Configure JWT Bearer as the default authentication/challenge scheme.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Minimal configuration for development: do not validate issuer/audience/signing key here.
    // Replace with real validation in production (TokenValidationParameters with IssuerSigningKey etc.).
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = false,
        ValidateLifetime = true
    };
});

// Dependency injection registrations

//builder.Services.AddTransient<AuthHeaderHandler>();
// Register Authentication Services
builder.Services.AddScoped<WebAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<WebAuthStateProvider>());
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

// Register DashboardService for role-based navigation
builder.Services.AddScoped<SMEapps.Shared.Services.DashboardService>();

var app = builder.Build();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

// Use CORS globally so preflight/options requests are handled and responses include the proper headers
app.UseCors("AllowClient");

// Enable authentication/authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(
        typeof(SMEapps.Shared._Imports).Assembly,
        typeof(SMEapps.Web.Client._Imports).Assembly);

app.Run();
