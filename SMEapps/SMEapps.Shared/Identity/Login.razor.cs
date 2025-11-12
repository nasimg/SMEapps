using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using SMEapps.Shared.Model;
using SMEapps.Shared.Services;
using System.Net.Http.Json;

namespace SMEapps.Shared.Identity
{
    public partial class Login : ComponentBase
    {
        private LoginModel loginModel = new();
        private bool isLoading = false;

        [Inject] public IHttpClientFactory HttpClientFactory { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;
        [Inject] public SMEapps.Shared.Services.ISStore SStore { get; set; } = default!;
        [Inject] ISnackbar Snackbar { get; set; } = default!;

        private HttpClient ApiClient => HttpClientFactory.CreateClient("ApiClient");

        private bool _processing = false;


        private async Task HandleValidSubmit(EditContext editContext)
        {
            isLoading = true;

          

            try
            {
                _processing = true;
                var response = await ApiClient.PostAsJsonAsync("Identity/GetToken", loginModel);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<LoginResult>();

                    if (result is not null && !string.IsNullOrEmpty(result.Token))
                    {
                        await SStore.SaveAsync("token", result.Token);
                        await SStore.SaveAsync("userName", result.UserName);
                        await SStore.SaveAsync("validity", result.Validity);
                        await SStore.SaveAsync("refreshToken", result.RefreshToken);
                        await SStore.SaveAsync("userId", result.UserId);
                        await SStore.SaveAsync("emailId", result.EmailId);
                        await SStore.SaveAsync("roleId", result.RoleId);
                        await SStore.SaveAsync("expiredTime", result.ExpiredTime.ToString("o"));
                        await SStore.SaveAsync("roleName", result.RoleName);
                        await SStore.SaveAsync("id", result.Id);
                        NavigationManager.NavigateTo("/");
                        Snackbar.Add("Logged in successfully.", Severity.Success, c =>
                        {
                            c.SnackbarVariant = Variant.Filled;
                            c.VisibleStateDuration = 3000;
                        });



                    }
                    else
                    {
                        Snackbar.Add("Invalid username or password", Severity.Error, c =>
                        {
                            c.SnackbarVariant = Variant.Filled;
                            c.VisibleStateDuration = 3000;
                        });

                    }
                }
                else
                {
                    Snackbar.Add("Login Failed! No Token Received", Severity.Error, c =>
                    {
                        c.SnackbarVariant = Variant.Filled;
                        c.VisibleStateDuration = 3000;
                    });

                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Login Failed! {ex}", Severity.Error, c =>
                {
                    c.SnackbarVariant = Variant.Filled;
                    c.VisibleStateDuration = 3000;
                });
            }

            _processing = false;
        }
    }
}
