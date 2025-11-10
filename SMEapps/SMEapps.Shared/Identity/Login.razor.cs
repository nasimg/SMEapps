using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SMEapps.Shared.Model;
using System.Net.Http.Json;

namespace SMEapps.Shared.Identity
{
    public partial class Login : ComponentBase
    {
        private LoginModel loginModel = new();
        private string? errorMessage;
        private bool isLoading = false;

        [Inject] public IHttpClientFactory HttpClientFactory { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;

        private HttpClient ApiClient => HttpClientFactory.CreateClient("ApiClient");

        private async Task HandleValidSubmit(EditContext editContext)
        {
            isLoading = true;
            errorMessage = null;

            try
            {
                // Call a simple login API that just checks credentials
                var response = await ApiClient.PostAsJsonAsync("Identity/Login", loginModel);

                if (response.IsSuccessStatusCode)
                {
                    // Option 1: The API returns a simple success message
                    var result = await response.Content.ReadAsStringAsync();

                    if (result.Contains("success", StringComparison.OrdinalIgnoreCase))
                    {
                        // Navigate to home or dashboard
                        NavigationManager.NavigateTo("/dashboard", true);
                    }
                    else
                    {
                        errorMessage = "Invalid email or password.";
                    }
                }
                else
                {
                    errorMessage = "Login failed. Please check your credentials.";
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Error: {ex.Message}";
            }

            isLoading = false;
        }
    }
}
