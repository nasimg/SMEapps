using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace SMEapps.Shared.Identity
{
    public partial class ForgotPassword
    {
        private EmailModel emailModel = new();
        private bool isLoading = false;

        //[Inject] private IHttpClientFactory ClientFactory { get; set; } = default!;
        //[Inject] private NavigationManager Nav { get; set; } = default!;
   

        private async Task SendResetEmail()
        {
            isLoading = true;
            try
            {
                var client = ClientFactory.CreateClient("ApiClient");
                var response = await client.GetAsync($"Identity/GetForgotToken/{emailModel.Email}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<Responses>();

                    if (result != null && result.IsSuccess)
                    {
                        ToastService.ShowSuccess("Token generated successfully!");

                        await Task.Delay(1000);
                        Nav.NavigateTo($"/identity/reset-password?email={Uri.EscapeDataString(result.Email ?? emailModel.Email)}&token={Uri.EscapeDataString(result.Token ?? "")}");
                    }
                    else
                    {
                        ToastService.ShowError(result?.Message ?? "Something went wrong.");
                    }
                }
                else
                {
                    ToastService.ShowError("Email not found.");
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

        public class EmailModel
        {
            [Required, EmailAddress]
            public string Email { get; set; } = "";
        }
    }

    public class Responses
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = "";
        public string ReturnCode { get; set; } = "";
        public string? Token { get; set; }
        public string? Email { get; set; }
    }
}
