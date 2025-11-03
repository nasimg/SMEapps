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

        [Inject] public IHttpClientFactory HttpClientFactory { get; set; } 
        [Inject] public SMEapps.Shared.Services.ISStore SStore { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; }
        private HttpClient ApiClient => HttpClientFactory.CreateClient("ApiClient");


        private async Task HandleValidSubmit(EditContext editContext)
        {
          
            try
            {

                errorMessage = string.Empty;

                var response = await ApiClient.PostAsJsonAsync("Identity/GetToken", loginModel);
                if (response.IsSuccessStatusCode) {
                    var result = await response.Content.ReadFromJsonAsync<LoginResult>();

                    if (result is not null && !string.IsNullOrEmpty(result.Token))
                    {
                        var loggedInUser = new LoginResult
                        {
                            Token = result.Token,
                            UserName = result.UserName ?? "",
                            Validity = result.Validity ?? "",
                            RefreshToken = result.RefreshToken ?? "",
                            UserId = result.UserId ?? "",
                            EmailId = result.EmailId ?? "",
                            RoleId = result.RoleId ?? "",
                            ExpiredTime = result.ExpiredTime,
                            RoleName = result.RoleName ?? "",
                            Id = result.Id ?? ""
                        };
                        await SStore.SaveAsync("SMEuser", loggedInUser);
                        NavigationManager.NavigateTo("/");
                        errorMessage = $"login Successful";

                    }
                    else
                    {
                        errorMessage = "Wrong credentials!";
                    }
                }
                else
                {
                    errorMessage = "Login Failed! No Token Received";
                }
              
            }
            catch (Exception ex)
            {
                errorMessage = "Login failed: " + ex.Message;
            }
        }
      
    }
}
