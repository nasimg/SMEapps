using SMEapps.Shared.Model;
using System.Net.Http.Json;

namespace SMEapps.Shared.Services
{
    public class FeaturesService
    {
        public readonly IHttpClientFactory _httpClientFactory;
        public FeaturesService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public HttpClient ApiClient => _httpClientFactory.CreateClient("ApiClient");

        public async Task<FeatureModel> PostFeatureAsync(FeatureModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var featureResponse = await ApiClient.PostAsJsonAsync("sme/api/Feature/SaveAsyn", model);

            var features = await featureResponse.Content.ReadFromJsonAsync<FeatureModel>();
            return features;

        }

        public async Task<List<FeatureModel>> GetAllFeatures()
        {

            var features = await ApiClient.GetFromJsonAsync<List<FeatureModel>>($"sme/api/Feature/GetAll");
            return features;
        }

        public async Task<FeatureModel> GetFeatureByIdAsync(int id)
        {
            if (id <=0) throw new ArgumentOutOfRangeException(nameof(id));

            // attempt to get by id from API
            var features = await ApiClient.GetFromJsonAsync<FeatureModel>($"sme/api/Feature/GetFilteredByKeysAsyn/{id}");
            return features;
        }

        public async Task<FeatureModel> UpdateFeatureAsync(FeatureModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var response = await ApiClient.PutAsJsonAsync("sme/api/Feature/UpdateAsyn", model);
            response.EnsureSuccessStatusCode();

            var updated = await response.Content.ReadFromJsonAsync<FeatureModel>();
            return updated;
        }

        public async Task<bool> DeleteFeatureAsync(int id)
        {
            if (id <=0) throw new ArgumentOutOfRangeException(nameof(id));

            var response = await ApiClient.DeleteAsync($"sme/api/Feature/DeleteAsyn/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<ModuleModel>> GetSelectedModule()
        {

            var module = await ApiClient.GetFromJsonAsync<List<ModuleModel>>($"sme/api/Module/GetAll");
            return module;
        }
    }
}
