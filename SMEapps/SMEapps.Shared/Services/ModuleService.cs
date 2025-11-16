using SMEapps.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SMEapps.Shared.Services
{
    public class ModuleService
    {
        public readonly IHttpClientFactory _httpClientFactory;
        public ModuleService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public HttpClient ApiClient => _httpClientFactory.CreateClient("ApiClient");

        public async Task<ModuleModel> PostModuleAsync(ModuleModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var moduleResponse = await ApiClient.PostAsJsonAsync("sme/api/Module/SaveAsyn", model);

            var module = await moduleResponse.Content.ReadFromJsonAsync<ModuleModel>();
            return module;

        }

        public async Task<List<ModuleModel>> GetAllModule()
        {

            var module = await ApiClient.GetFromJsonAsync<List<ModuleModel>>($"sme/api/Module/GetAll");
            return module;
        }

        public async Task<ModuleModel> GetModuleByIdAsync(int id)
        {
            if (id <=0) throw new ArgumentOutOfRangeException(nameof(id));

            // attempt to get by id from API
            var module = await ApiClient.GetFromJsonAsync<ModuleModel>($"sme/api/Module/GetById/{id}");
            return module;
        }

        public async Task<ModuleModel> UpdateModuleAsync(ModuleModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var response = await ApiClient.PutAsJsonAsync("sme/api/Module/UpdateAsync", model);
            response.EnsureSuccessStatusCode();

            var updated = await response.Content.ReadFromJsonAsync<ModuleModel>();
            return updated;
        }

        public async Task<bool> DeleteModuleAsync(int id)
        {
            if (id <=0) throw new ArgumentOutOfRangeException(nameof(id));

            var response = await ApiClient.DeleteAsync($"sme/api/Module/DeleteAsyn/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
