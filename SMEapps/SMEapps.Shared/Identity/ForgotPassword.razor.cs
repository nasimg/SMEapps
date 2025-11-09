using Microsoft.AspNetCore.Components;
using MudBlazor;
using SMEapps.Shared.Model;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;

namespace SMEapps.Shared.Identity;

public partial class ForgotPassword : ComponentBase, IDisposable
{
    [Inject] private IHttpClientFactory ClientFactory { get; set; } = default!;
    [Inject] private NavigationManager Nav { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    private HttpClient Api => ClientFactory.CreateClient("ApiClient");

    private readonly EmailModel _model = new();
    private bool _isLoading;

    protected async Task HandleValidSubmit()
    {
        _isLoading = true;
        StateHasChanged();

        try
        {
            var response = await Api.GetAsync($"Identity/GetForgotToken/{Uri.EscapeDataString(_model.Email)}");

            if (!response.IsSuccessStatusCode)
            {
                Snackbar.Add("Email not found.", Severity.Error);
                return;
            }

            var result = await response.Content.ReadFromJsonAsync<Responses>();

            if (result?.IsSuccess == true)
            {
                Snackbar.Add("Reset link generated!", Severity.Success);
                await Task.Delay(800);

                var uri = Nav.ToAbsoluteUri(
                    $"/identity/reset-password?email={Uri.EscapeDataString(result.Email ?? _model.Email)}" +
                    $"&token={Uri.EscapeDataString(result.Token ?? "")}");

                Nav.NavigateTo(uri.ToString());
            }
            else
            {
                Snackbar.Add(result?.Message ?? "Something went wrong.", Severity.Error);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error: {ex.Message}", Severity.Error);
        }
        finally
        {
            _isLoading = false;
            StateHasChanged();
        }
    }

    public void Dispose() { /* HttpClient is managed by the factory */ }

    // --------------------------------------------------------------------
    // Inner model – validation attributes are honoured by <DataAnnotationsValidator />
    // --------------------------------------------------------------------
    private sealed class EmailModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;
    }
}