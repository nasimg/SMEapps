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
    }
}
