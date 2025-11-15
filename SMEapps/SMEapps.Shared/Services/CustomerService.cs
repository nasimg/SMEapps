using Microsoft.AspNetCore.Components;
using SMEapps.Shared.Model;
using System.Net.Http.Json;

namespace SMEapps.Shared.Services;

public class CustomerService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CustomerService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    private HttpClient ApiClient => _httpClientFactory.CreateClient("ApiClient");
 
    public async Task<object> PostPersonalInfo(PersonalInfo model)
    {
        if (model == null) throw new ArgumentNullException(nameof(model));

        try
        {
            var personResponse = await ApiClient.PostAsJsonAsync("sme/api/PersonInfo/SaveAsyn", model);

            if (!personResponse.IsSuccessStatusCode)
            {
                // Optionally you could read error details here
                return null;
            }

            var person = await personResponse.Content.ReadFromJsonAsync<object>();
            return person;
        }
        catch
        {

            return null;
        }
    }
}

