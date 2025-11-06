using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;

namespace SMEapps.Shared.Services;

public class DashboardService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public DashboardService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    private HttpClient ApiClient => _httpClientFactory.CreateClient("ApiClient");
 
    public async Task<List<Model.ModuleWithFeatures>> GetModulesByRoleNameAsync(string roleName)
    {


        var url = $"sme/api/RoleFeature/GetModulesByRoleName/modules/{Uri.EscapeDataString(roleName)}";

        try
        {
            var response = await ApiClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return new List<Model.ModuleWithFeatures>();
            }

            var modules = await response.Content.ReadFromJsonAsync<List<Model.ModuleWithFeatures>>();
            return modules ?? new List<Model.ModuleWithFeatures>();
        }
        catch
        {
            return new List<Model.ModuleWithFeatures>();
        }
    }
}

