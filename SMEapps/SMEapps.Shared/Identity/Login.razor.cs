using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SMEapps.Shared.Model;
using SMEapps.Shared.Services;
using System.Net.Http.Json;


namespace SMEapps.Shared.Identity
{
    public partial class Login
    {
        
        private LoginModel loginModel = new LoginModel();
        private string? errorMessage = string.Empty;

        [Inject] public IHttpClientFactory HttpClientFactory { get; set; } = default!;
        [Inject] public SMEapps.Shared.Services.ISStore SStore { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;
        private HttpClient ApiClient => HttpClientFactory.CreateClient("ApiClient");


        private async Task HandleValidSubmit(EditContext editContext)
        {

            try
            {
                var response = await ApiClient.PostAsJsonAsync("Identity/GetToken", loginModel);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<LoginResult>();
                    if (result is not null && !string.IsNullOrEmpty(result.Token))
                    {
                        // Securely store the JWT token
                        await SStore.SaveAsync("token", result.Token);
                        await SStore.SaveAsync("userName", result.UserName);
                        await SStore.SaveAsync("validity", result.Validity);
                        await SStore.SaveAsync("refreshToken", result.RefreshToken);
                        await SStore.SaveAsync("userId", result.UserId);
                        await SStore.SaveAsync("emailId", result.EmailId);
                        await SStore.SaveAsync("roleId", result.RoleId);
                        await SStore.SaveAsync("expiredTime", result.ExpiredTime.ToString("o")); // ISO 8601 format
                        await SStore.SaveAsync("roleName", result.RoleName);
                        await SStore.SaveAsync("id", result.Id);

                        // Clear cache to refresh user-specific data
                        //DataCache.ClearAllCache();

                        // Notify authentication state change
                        // Use reflection to call NotifyUserAuthenticationAsync if available
                        var notifyMethod = AuthProvider.GetType().GetMethod("NotifyUserAuthenticationAsync");
                        if (notifyMethod != null)
                        {
                            var notifyTask = notifyMethod.Invoke(AuthProvider, null) as Task;
                            if (notifyTask != null)
                            {
                                await notifyTask;
                            }
                        }

                        NavigationManager.NavigateTo("/");
                        //color = "green";
                        errorMessage = $"login Successful";
                        loginModel = new();
                    }
                    else
                    {
                        errorMessage = "Login failed: No token received.";
                    }
                }
                else
                {
                    errorMessage = "Invalid login attempt.";
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Login failed: {ex.Message}";
            }
        }
      
    }
}
