using Microsoft.JSInterop;
using SMEapps.Shared.Services;
using System.Text.Json;

namespace SMEapps.Web.Client.Services.SStore
{
    public class SStore : ISStore
    {
        private readonly IJSRuntime _js;

        public SStore(IJSRuntime js)
        {
            _js = js;
        }

        private async Task SafeInvokeAsync(Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (JSDisconnectedException) { /* Ignore: app is reloading */ }
            catch (ObjectDisposedException) { /* Ignore: JS runtime disposed */ }
        }

        public async Task<bool> SaveAsync<T>(string key, T value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value), "Value cannot be null.");

            var json = JsonSerializer.Serialize(value);
            await SafeInvokeAsync(() => _js.InvokeVoidAsync("localStorage.setItem", key, json).AsTask());
            return true;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            try
            {
                var json = await _js.InvokeAsync<string>("localStorage.getItem", key);
                return json == null ? default : JsonSerializer.Deserialize<T>(json);
            }
            catch (JSDisconnectedException) { }
            catch (ObjectDisposedException) { }

            return default;
        }

        public async Task<bool> RemoveAsync(string key)
        {
            await SafeInvokeAsync(() => _js.InvokeVoidAsync("localStorage.removeItem", key).AsTask());
            return true;
        }

        public async Task ClearAsync()
        {
            await SafeInvokeAsync(() => _js.InvokeVoidAsync("localStorage.clear").AsTask());
        }

        public async Task<bool> ContainsKeyAsync(string key)
        {
            try
            {
                var value = await _js.InvokeAsync<string>("localStorage.getItem", key);
                return value != null;
            }
            catch (JSDisconnectedException) { }
            catch (ObjectDisposedException) { }

            return false;
        }
    }
}
