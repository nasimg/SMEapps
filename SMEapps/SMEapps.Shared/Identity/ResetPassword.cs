using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SMEapps.Shared.Identity
{
    public partial class ResetPassword
    {
        private ResetPasswordModel resetModel = new();
        private bool isLoading;

        protected override void OnInitialized()
        {
            var uri = Nav.ToAbsoluteUri(Nav.Uri);
            var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
            resetModel.Email = query["email"];
            resetModel.ResetPasswordToken = query["token"];
        }

        private async Task ResetPasswordHandler()
        {
            if (resetModel.NewPassword != resetModel.ConfirmPassword)
            {
                ToastService.ShowError("Passwords do not match!");
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
                        await Task.Delay(1000);
                        ToastService.ShowSuccess("Password reset successfully!");
                        Nav.NavigateTo("/identity/login");
                    }
                    else
                    {
                        ToastService.ShowError(result?.Message ?? "Failed to reset password.");
                    }
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    ToastService.ShowError($"Server error: {response.StatusCode} - {error}");
                }
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Error: {ex.Message}");
            }
            finally
            {
                isLoading = false;
            }
        }


        public class ResetPasswordModel
        {
            public string Email { get; set; } = "";
            public string ResetPasswordToken { get; set; } = "";
            public string NewPassword { get; set; } = "";
            public string ConfirmPassword { get; set; } = "";
        }
    }
    }
