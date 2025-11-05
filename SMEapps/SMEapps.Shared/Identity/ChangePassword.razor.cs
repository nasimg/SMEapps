using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SMEapps.Shared.Model;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;

namespace SMEapps.Shared.Identity
{
    public partial class ChangePassword : ComponentBase
    {
 
        protected ChangePasswordModel ChangePassmodel { get; set; } = new();

        protected string? Message;
        protected bool IsSuccess;
        protected bool isLoading = false;

        private bool showOldPassword = false;
        private bool showNewPassword = false;
        private bool showConfirmPassword = false;

  
        protected async Task HandleChangePassword()
        {
            isLoading = true;
            Message = null;
            StateHasChanged();

            try
            {
                var client = ClientFactory.CreateClient("ApiClient");

                var payload = new
                {
                    Email = ChangePassmodel.Email,
                    OldPassword = ChangePassmodel.OldPassword,
                    NewPassword = ChangePassmodel.NewPassword,
                };


                var response = await client.PostAsJsonAsync("Identity/ChangePassword", payload);

                if (response.IsSuccessStatusCode)
                {
                    IsSuccess = true;
                    Message = "Password changed successfully!";
                    ToastService.ShowSuccess(Message);

                    ChangePassmodel = new();               
                    await Task.Delay(1500);
                    Nav.NavigateTo("/");
                }
                else
                {
                    IsSuccess = false;
                    var err = await response.Content.ReadAsStringAsync();
                    Message = $"Failed to change password ({(int)response.StatusCode})";
                    ToastService.ShowError(Message);
                    Console.WriteLine(err);
                }
            }
            catch (Exception ex)
            {
                IsSuccess = false;
                Message = "An unexpected error occurred.";
                ToastService.ShowError(Message);
                Console.WriteLine(ex);
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }

        private void Cancel()
        {
            Nav.NavigateTo("/");
        }

        
    }
}