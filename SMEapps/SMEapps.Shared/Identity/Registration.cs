using Microsoft.FluentUI.AspNetCore.Components;
using SMEapps.Shared.Model;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace SMEapps.Shared.Identity
{
    public partial class Registration
    {
        private RegisterModel Model = new();
        private FluentToast? toast;

        [Inject] private IHttpClientFactory ClientFactory { get; set; } = default!;
        [Inject] private NavigationManager NavManager { get; set; } = default!;
        [Inject] private IToastService Toast { get; set; } = default!;

        private HttpClient ApiClient => ClientFactory.CreateClient("ApiClient");

        private async Task HandleSubmit()
        {
            
            try
            {
                if (Model.Password != Model.ConfirmPassword)
                {
                    Toast.ShowError("Passwords do not match!");
                    return;
                }

                var requestBody = new
                {
                    email = Model.Email,
                    password = Model.Password,
                    //confirmPassword = Model.ConfirmPassword,
                    phoneNumber = Model.PhoneNumber,
                    roleName = "user",
                    confirmUrl = "https://localhost:7187"
                };

                var response = await ApiClient.PostAsJsonAsync("identity/createuser", requestBody);

                if (response.IsSuccessStatusCode)
                {
                    Toast.ShowSuccess(
                        title: "Registration successful!",
                        timeout: 2000
                    );

                    NavManager.NavigateTo("/identity/login");
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Toast.ShowError($"Registration failed: {error}");
                }
            }
            catch (Exception ex)
            {
                Toast.ShowError($"Error: {ex.Message}");
            }
        }
    }
}
