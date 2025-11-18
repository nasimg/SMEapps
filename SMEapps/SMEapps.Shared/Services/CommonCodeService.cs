using SMEapps.Shared.Model;
using System.Net.Http.Json;

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
    public async Task<ApiResponseModel> SaveAsync(CommonCode model)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "sme/api/CommonCode/Save",
            model
        );

        if (!response.IsSuccessStatusCode)
        {
            return new ApiResponseModel
            {
                StatusCode = (int)response.StatusCode,
                Message = "Failed to save common code."
            };
        }

        return await response.Content.ReadFromJsonAsync<ApiResponseModel>()
               ?? new ApiResponseModel
               {
                   StatusCode = 500,
                   Message = "Invalid save response."
               };
    }
    public async Task<ApiResponseModel> DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"sme/api/CommonCode/Delete/{id}");

        if (!response.IsSuccessStatusCode)
        {
            return new ApiResponseModel
            {
                StatusCode = (int)response.StatusCode,
                Message = "Failed to delete common code."
            };
        }

        return await response.Content.ReadFromJsonAsync<ApiResponseModel>()
               ?? new ApiResponseModel
               {
                   StatusCode = 500,
                   Message = "Invalid delete response."
               };
    }

    public async Task<List<CommonCodeModel>> GetCommonCode()
    {
        var response = await _httpClient.GetAsync("sme/api/CommonCode/GetAll");

        if (!response.IsSuccessStatusCode)
        {
            return new List<CommonCodeModel>();
        }

        var result = await response.Content.ReadFromJsonAsync<List<CommonCodeModel>>();
        return result;

    }

    public class CommonCode
    {
        public int CommonCodeId { get; set; }
        public string CodeType { get; set; } = string.Empty;
        public string CodeValue { get; set; } = string.Empty;
        //public int? ParentId { get; set; }

        public bool Status { get; set; } = true;  // ← NOT nullable
    }
    public class ApiResponseModel
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
    }

}
