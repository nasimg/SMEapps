using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
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
        [Inject] public AuthenticationStateProvider AuthProvider { get; set; } = default!;
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

                        // Notify authentication state change
                        // Use reflection to call NotifyUserAuthenticationAsync if available
                        var notifyMethod = AuthProvider.GetType().GetMethod("NotifyUserAuthenticationAsync");
                        bool notified = false;
                        if (notifyMethod != null)
                        {
                            try
                            {
                                var notifyTask = notifyMethod.Invoke(AuthProvider, null) as Task;
                                if (notifyTask != null)
                                {
                                    await notifyTask;
                                    notified = true;
                                }
                            }
                            catch
                            {
                                // ignore reflection invocation errors
                            }
                        }

                        // After successful login, navigate back to the original returnUrl (if present)
                        // Parse returnUrl from the current URI query string
                        string returnUrl = "/";
                        try
                        {
                            var abs = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
                            var query = abs.Query; // e.g. ?returnUrl=%2Fcounter
                            if (!string.IsNullOrEmpty(query))
                            {
                                var pairs = query.TrimStart('?').Split('&', StringSplitOptions.RemoveEmptyEntries);
                                foreach (var p in pairs)
                                {
                                    var kv = p.Split('=', 2);
                                    if (kv.Length == 2 && kv[0] == "returnUrl")
                                    {
                                        returnUrl = Uri.UnescapeDataString(kv[1]);
                                        break;
                                    }
                                }
                            }
                        }
                        catch
                        {
                            // ignore parsing errors and fall back to "/"
                        }

                        // If NotifyUserAuthenticationAsync was not available, give the browser a short moment
                        // to persist values to localStorage so subsequent auth checks see them.
                        if (!notified)
                        {
                            await Task.Delay(200);
                        }

                        // Force full reload so that the app picks up the new token reliably
                        var target = string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl;
                        NavigationManager.NavigateTo(target, true);

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
