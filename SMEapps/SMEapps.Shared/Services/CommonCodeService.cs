using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SMEapps.Shared.Services;

public class CommonCodeService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CommonCodeService(IHttpClientFactory httpClient)
    {
        _httpClientFactory = httpClient;
    }

    private HttpClient ApiClient => _httpClientFactory.CreateClient("ApiClient");

    public async Task<List<Model.CommonCodeModel>> GetCommonCode()
    {
        try
        {

        }catch(Exception ex)
        {
            var response = await ApiClient.GetAsync("")
        }

    }


}
