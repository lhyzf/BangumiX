using Bangumi.Api.Models;
using BangumiX.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BangumiX.Services
{
    public class CalendarDataStore : IDataStore<Calendar>
    {
        private readonly List<Calendar> items;

        public CalendarDataStore()
        {
            items = new List<Calendar>();
            foreach (var item in Bangumi.Api.BangumiApi.BgmCache.Calendar())
            {
                items.Add(item);
            }
        }

        public Task<bool> AddItemAsync(Calendar item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Calendar> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Calendar>> GetItemsAsync(bool forceRefresh = false)
        {
            if (forceRefresh)
            {
                items.Clear();
                foreach (var item in await Bangumi.Api.BangumiApi.BgmApi.Calendar())
                {
                    items.Add(item);
                }
            }
            return await Task.FromResult(items);
        }

        public Task<bool> UpdateItemAsync(Calendar item)
        {
            throw new NotImplementedException();
        }
    }
}
