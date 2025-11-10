
using SMEapps.Shared.Services;

public class SStore : ISStore
{
    private readonly Dictionary<string, object> _store = new();
    public Task<bool> SaveAsync<T>(string key, T value)
    {
        _store[key] = value ?? throw new ArgumentNullException(nameof(value), "Value cannot be null.");
        return Task.FromResult(true);
    }
    public Task<T?> GetAsync<T>(string key)
    {
        if (_store.TryGetValue(key, out var value) && value is T typedValue)
        {
            return Task.FromResult<T?>(typedValue);
        }
        return Task.FromResult<T?>(default);
    }
    public Task<bool> RemoveAsync(string key)
    {
        return Task.FromResult(_store.Remove(key));
    }
    public Task ClearAsync()
    {
        _store.Clear();
        return Task.CompletedTask;
    }
    public Task<bool> ContainsKeyAsync(string key)
    {
        return Task.FromResult(_store.ContainsKey(key));
    }
}