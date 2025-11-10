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

        private static bool IsUnsupportedJsRuntime(Exception? ex)
        {
            while (ex != null)
            {
                var fullName = ex.GetType().FullName ?? string.Empty;
                var name = ex.GetType().Name ?? string.Empty;

                if (fullName == "Microsoft.AspNetCore.Components.Endpoints.UnsupportedJavaScriptRuntime"
                    || name.Contains("UnsupportedJavaScriptRuntime")
                    || (ex.Message != null && ex.Message.Contains("JavaScript interop calls cannot be issued during server-side static rendering", StringComparison.OrdinalIgnoreCase)))
                {
                    return true;
                }

                ex = ex.InnerException;
            }

            return false;
        }

        private async Task SafeInvokeAsync(Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (JSDisconnectedException) { /* Ignore: app is reloading */ }
            catch (ObjectDisposedException) { /* Ignore: JS runtime disposed */ }
            catch (Exception ex) when (IsUnsupportedJsRuntime(ex)) { /* Ignore: running in environment without JS runtime (prerendering) */ }
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
            catch (Exception ex) when (IsUnsupportedJsRuntime(ex)) { }

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
            catch (Exception ex) when (IsUnsupportedJsRuntime(ex)) { }

            return false;
        }
    }
}
