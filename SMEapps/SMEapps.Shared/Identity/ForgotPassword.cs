using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMEapps.Shared.Identity
{
    public partial class ForgotPassword
    {
        private EmailModel emailModel = new();
        private bool isLoading = false;

        private async Task SendResetEmail()
        {
            isLoading = true;
            try
            {
                var client = ClientFactory.CreateClient("ApiClient");
                var response = await client.GetAsync($"Identity/GetForgotToken/{emailModel.Email}");

                if (response.IsSuccessStatusCode)
                {
                    ToastService.ShowSuccess("Password reset link has been sent to your email.");
                }
                else
                {
                    ToastService.ShowError("Unable to send reset link. Please check your email address.");
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
            public string Email { get; set; } = string.Empty;
        }
    }
    }
