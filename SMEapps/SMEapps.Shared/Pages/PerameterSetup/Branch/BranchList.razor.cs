using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using SMEapps.Shared.Model;
using SMEapps.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SMEapps.Shared.Pages.PerameterSetup.Branch
{
    public partial class  BranchList
    {
        [Inject] private IHttpClientFactory HttpClientFactory { get; set; } = default!;
        [Inject] public SMEapps.Shared.Services.ISStore SStore { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;
        private HttpClient ApiClient => HttpClientFactory.CreateClient("ApiClient");
        private List<BranchList> Branches = new List<BranchList>();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await InitializeUserContextAsync();

           await LoadBranchesAsync();
            
        }

        private async Task InitializeUserContextAsync()
        {
            var user = await SStore.GetAsync<LoginResult>("SMEuser");
            var token = string.Empty;
            if (user != null)
            {
                //var user = JsonSerializer.Deserialize<LoginResult>(json);
                token = user?.Token;
            }
            //userId = await SStore.GetAsync<string>("userName") ?? "System";
            //orgId = await DataCacheService.GetUserDefaultOrganizationIdAsync() ?? 0;

            //MenuState.IsExpanded = false;

            if (string.IsNullOrWhiteSpace(token))
            {
                //DialogService.ShowError("You must be logged in to access this page.");
                NavigationManager.NavigateTo("/identity/login");
                throw new UnauthorizedAccessException("No authentication token found");
            }

            // Setup HTTP client authorization and create member service using backward compatibility constructor
            ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //memberService = new MemberService(ApiClient, token);
        }

        private async Task LoadBranchesAsync()
        {
            var response = await ApiClient.GetAsync("/sme/api/BranchInfo/GetAll");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<BranchList>>();
                if (result != null)
                {
                    Branches = result;
                }
            }
        }


    }
}
