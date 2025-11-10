using Microsoft.AspNetCore.Components;
using MudBlazor;
using SMEapps.Shared.Model;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SMEapps.Shared.Identity
{
    public partial class ResetPassword : ComponentBase
    {
        protected ResetPasswordModel resetModel = new();
        protected bool isLoading = false;

        [Inject] protected NavigationManager Nav { get; set; } = default!;
        [Inject] protected IHttpClientFactory ClientFactory { get; set; } = default!;
        [Inject] protected ISnackbar Snackbar { get; set; } = default!;

        protected override void OnInitialized()
        {
            try
            {
                var uri = Nav.ToAbsoluteUri(Nav.Uri);
                var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
                resetModel.Email = query["email"];
                resetModel.ResetPasswordToken = query["token"];

                if (string.IsNullOrWhiteSpace(resetModel.Email) || string.IsNullOrWhiteSpace(resetModel.ResetPasswordToken))
                {
                    Snackbar.Add("Invalid reset link.", Severity.Error);
                    Nav.NavigateTo("/identity/login");
                }
            }
            catch
            {
                Snackbar.Add("Invalid link format.", Severity.Error);
                Nav.NavigateTo("/identity/login");
            }
        }

        protected async Task ResetPasswordHandler()
        {
            Snackbar.Add("ResetPasswordHandler called.", Severity.Info); // test line
            if (resetModel.NewPassword != resetModel.ConfirmPassword)
            {
                Snackbar.Add("Passwords do not match!", Severity.Error);
                return;
            }

            await SubmitResetPassword();
        }

        private async Task SubmitResetPassword()
        {
            isLoading = true;
            try
            {
                var client = ClientFactory.CreateClient("ApiClient");

                var payload = new
                {
                    Email = resetModel.Email,
                    ResetPasswordToken = resetModel.ResetPasswordToken,
                    NewPassword = resetModel.NewPassword
                };

              
                var response = await client.PostAsJsonAsync("Identity/ForgotPassword", payload);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<Responses>();
                    if (result?.IsSuccess == true)
                    {
                        Snackbar.Add("Password reset successfully!", Severity.Success);
                        await Task.Delay(1000);
                        Nav.NavigateTo("/identity/login");
                    }
                    else
                    {
                        Snackbar.Add(result?.Message ?? "Failed to reset password.", Severity.Error);
                    }
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Snackbar.Add($"Server error: {response.StatusCode} - {error}", Severity.Error);
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error: {ex.Message}", Severity.Error);
            }
            finally
            {
                isLoading = false;
            }
        }
    }
}
