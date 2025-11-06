using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using SMEapps.Shared.Model;
using SMEapps.Shared.Services;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net;

namespace SMEapps.Shared.Pages.PerameterSetup.Branch
{
    public partial class BranchList
    {
        [Inject] private IHttpClientFactory HttpClientFactory { get; set; } = default!;
        [Inject] public ISStore SStore { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;

        private HttpClient ApiClient => HttpClientFactory.CreateClient("ApiClient");
        private List<BranchList> Branches = new();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                // Check token first
                var hasToken = await GetOrRedirectAsync();
                if (!hasToken)
                    return;

                // Load data if token is valid
                await LoadBranchesAsync();
                StateHasChanged();
            }
        }

        private async Task LoadBranchesAsync()
        {
            try
            {
                var token = await SStore.GetAsync<string>("token");
                ApiClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await ApiClient.GetAsync("/sme/api/BranchInfo/GetAll");

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await SStore.RemoveAsync("token");
                    NavigationManager.NavigateTo("/identity/login", true);
                    return;
                }

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<List<BranchList>>();
                    if (result != null)
                        Branches = result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading branches: {ex.Message}");
                // Optionally redirect or show error
            }
        }

        private async Task<bool> GetOrRedirectAsync()
        {
            var token = await SStore.GetAsync<string>("token");

            if (string.IsNullOrEmpty(token))
            {
                NavigationManager.NavigateTo("/identity/login", true);
                return false;
            }

            return true;
        }
    }
}
