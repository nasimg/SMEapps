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


        [Inject] private IHttpClientFactory ClientFactory { get; set; } = default!;
        [Inject] private NavigationManager NavManager { get; set; } = default!;


        private HttpClient ApiClient => ClientFactory.CreateClient("ApiClient");

        private async Task HandleSubmit()
        {
            
            try
            {
                if (Model.Password != Model.ConfirmPassword)
                {
              
                    return;
                }

                var requestBody = new
                {
                    email = Model.Email,
                    password = Model.Password,
                    //confirmPassword = Model.ConfirmPassword,
                    phoneNumber = Model.PhoneNumber,
                    roleName = "user",
                    confirmUrl = "https://localhost:7187"
                };

                var response = await ApiClient.PostAsJsonAsync("identity/createuser", requestBody);

                if (response.IsSuccessStatusCode)
                {
                   

                    NavManager.NavigateTo("/identity/login");
                }
                else
                {
                  
                }
            }
            catch (Exception ex)
            {
           
            }
        }
    }
}
