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
        private List<BranchList> Branches = new ();

        protected override async Task OnInitializedAsync() 
        {

           await LoadBranchesAsync();            
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
