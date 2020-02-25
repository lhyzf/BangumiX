using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BangumiX.Services
{
    public interface ITypeDataStore<T>
    {
        Task<bool> AddItemAsync(T item);
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync(string id);
        Task<T> GetItemAsync(string id);
        Task<IEnumerable<T>> GetItemsAsync(string type, bool forceRefresh = false);
    }
}
