using SMEapps.Shared.Model;
using System.Net.Http.Json;

namespace SMEapps.Shared.Services;

public class CommonCodeService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CommonCodeService(IHttpClientFactory httpClient)
    {
        _httpClientFactory = httpClient;
    }

    private HttpClient ApiClient => _httpClientFactory.CreateClient("ApiClient");

    public async Task<List<CommonCodeModel>> GetCommonCode()
    {
        var response = await ApiClient.GetAsync("sme/api/CommonCode/GetAll");

        if (!response.IsSuccessStatusCode)
        {
            return new List<CommonCodeModel>();
        }

        var result = await response.Content.ReadFromJsonAsync<List<CommonCodeModel>>();
        return result;

    }


}
