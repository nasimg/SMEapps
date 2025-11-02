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

        private HttpClient ApiClient => HttpClientFactory.CreateClient("ApiClient");

        private async Task HandleSubmit()
        {
            try
            {
                

                var requestBody = new
                {
                    email = Model.Email,
                    password = Model.Password,
                    phoneNumber = Model.PhoneNumber,
                    roleName = "user",
                    confirmUrl = "https://localhost:7187"
                };

                var response = await ApiClient.PostAsJsonAsync("identity/createuser", requestBody);

                if (response.IsSuccessStatusCode)
                {
                    ToastService.ShowSuccess(
                        title: "Registration successful!",
                        timeout: 5000
                    );

                    Navigation.NavigateTo("/identity/login");
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    ToastService.ShowError($"Registration failed: {error}");
                }
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Error: {ex.Message}");
            }
        }
    }
}
