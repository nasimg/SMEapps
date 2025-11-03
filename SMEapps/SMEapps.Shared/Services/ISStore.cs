using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMEapps.Shared.Services
{
    public interface ISStore
    {
        Task<bool> SaveAsync<T>(string key, T value);
        Task<T?> GetAsync<T>(string key);
        Task<bool> RemoveAsync(string key);
        Task ClearAsync();
        Task<bool> ContainsKeyAsync(string key);
        //Task<IEnumerable<string>> GetAllKeysAsync();
        //Task<int> CountAsync();
    }
}
