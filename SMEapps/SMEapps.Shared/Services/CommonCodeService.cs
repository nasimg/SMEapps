using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SMEapps.Shared.Services;

public class CommonCodeService
{
    private readonly HttpClient _httpClient;

    public CommonCodeService(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient("ApiClient");
    }

    public async Task<List<CommonCode>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<CommonCode>>("sme/api/CommonCode/GetAll");
    }

    public async Task<ResultViewModel> SaveAsync(CommonCode model)
    {
        var response = await _httpClient.PostAsJsonAsync(
           "sme/api/CommonCode/SaveAsyn",  // ← Fixed: was "SaveAsyn"
            model);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<ResultViewModel>()
                   ?? new ResultViewModel { StatusCode = 200 };
        }

        return new ResultViewModel
        {
            StatusCode = (int)response.StatusCode,
            Message = await response.Content.ReadAsStringAsync()
        };
    }
}

public class ResultViewModel
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
}

public class CommonCode
{
    public int CommonCodeId { get; set; }
    public string CodeType { get; set; } = string.Empty;
    public string CodeValue { get; set; } = string.Empty;
    public int? ParentId { get; set; }

    public bool Status { get; set; } = true;  // ← NOT nullable
}


