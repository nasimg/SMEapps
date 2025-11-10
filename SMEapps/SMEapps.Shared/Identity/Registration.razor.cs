using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;
using SMEapps.Shared.Model;
using System.Net.Http.Json;

namespace SMEapps.Shared.Identity;

public partial class Registration : ComponentBase, IDisposable
{
    [Inject] private IHttpClientFactory HttpClientFactory { get; set; } = default!;
    [Inject] private NavigationManager Nav { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    private HttpClient Api => HttpClientFactory.CreateClient("ApiClient");

    protected RegisterModel Model { get; set; } = new();
    protected bool IsLoading { get; set; }

    // Optional: inject the base URL instead of hard-coding it
    [Inject] private IConfiguration Config { get; set; } = default!;

    protected override void OnInitialized()
    {
        // If you want to pre-fill for debugging only – comment out in prod
        // Model.Email = "test@example.com";
    }

    protected async Task HandleValidSubmit()
    {
        IsLoading = true;
        StateHasChanged(); // force UI update immediately

        try
        {
            var payload = new
            {
                email = Model.Email,
                password = Model.Password,
                phoneNumber = Model.PhoneNumber,
                roleName = "user",
                confirmUrl = Config["Identity:ConfirmUrl"] ?? $"{Nav.BaseUri}identity/confirm"
            };

            var response = await Api.PostAsJsonAsync("identity/createuser", payload);

            if (response.IsSuccessStatusCode)
            {
                Snackbar.Add("Registration successful!", Severity.Success);
                await Task.Delay(800); // tiny pause for the snackbar
                Nav.NavigateTo("/identity/login");
            }
            else
            {
                var err = await response.Content.ReadAsStringAsync();
                Snackbar.Add($"Registration failed: {err}", Severity.Error);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Unexpected error: {ex.Message}", Severity.Error);
        }
        finally
        {
            IsLoading = false;
            StateHasChanged();
        }
    }

    public void Dispose()
    {
        // HttpClient from IHttpClientFactory is managed – nothing to dispose here.
    }
}