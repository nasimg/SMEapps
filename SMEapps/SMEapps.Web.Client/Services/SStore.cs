using Microsoft.JSInterop;
using SMEapps.Shared.Services;
using System.Text.Json;

namespace SMEapps.Web.Client.Services.SStore
{
    public class SStore : ISStore
    {
        //private readonly Dictionary<string, object> _store = new();
        //public Task<bool> SaveAsync<T>(string key, T value)
        //{
        //    _store[key] = value ?? throw new ArgumentNullException(nameof(value), "Value cannot be null.");
        //    return Task.FromResult(true);
        //}
        //public Task<T?> GetAsync<T>(string key)
        //{
        //    if (_store.TryGetValue(key, out var value) && value is T typedValue)
        //    {
        //        return Task.FromResult<T?>(typedValue);
        //    }
        //    return Task.FromResult<T?>(default);
        //}
        //public Task<bool> RemoveAsync(string key)
        //{
        //    return Task.FromResult(_store.Remove(key));
        //}
        //public Task ClearAsync()
        //{
        //    _store.Clear();
        //    return Task.CompletedTask;
        //}
        //public Task<bool> ContainsKeyAsync(string key)
        //{
        //    return Task.FromResult(_store.ContainsKey(key));
        //}
        private readonly IJSRuntime _js;

        public SStore(IJSRuntime js)
        {
            _js = js;
        }

        public async Task<bool> SaveAsync<T>(string key, T value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value), "Value cannot be null.");

            var json = JsonSerializer.Serialize(value);
            await _js.InvokeVoidAsync("localStorage.setItem", key, json);
            return true;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var json = await _js.InvokeAsync<string>("localStorage.getItem", key);
            if (json == null) return default;

            return JsonSerializer.Deserialize<T>(json);
        }

        public async Task<bool> RemoveAsync(string key)
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", key);
            return true;
        }

        public async Task ClearAsync()
        {
            await _js.InvokeVoidAsync("localStorage.clear");
        }

        public async Task<bool> ContainsKeyAsync(string key)
        {
            var value = await _js.InvokeAsync<string>("localStorage.getItem", key);
            return value != null;
        }
    }
}
