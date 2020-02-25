using Bangumi.Api.Models;
using BangumiX.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BangumiX.Services
{
    public class CollectionDataStore : ITypeDataStore<Collection>
    {
        private readonly Dictionary<SubjectType, CollectionE> items;

        public CollectionDataStore()
        {
            items = new Dictionary<SubjectType, CollectionE>();
            items.Add(SubjectType.Anime, Bangumi.Api.BangumiApi.BgmCache.Collections(SubjectType.Anime));
            items.Add(SubjectType.Book, Bangumi.Api.BangumiApi.BgmCache.Collections(SubjectType.Book));
            items.Add(SubjectType.Game, Bangumi.Api.BangumiApi.BgmCache.Collections(SubjectType.Game));
            items.Add(SubjectType.Music, Bangumi.Api.BangumiApi.BgmCache.Collections(SubjectType.Music));
            items.Add(SubjectType.Real, Bangumi.Api.BangumiApi.BgmCache.Collections(SubjectType.Real));
        }

        public Task<bool> AddItemAsync(Collection item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Collection> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Collection>> GetItemsAsync(string type, bool forceRefresh = false)
        {
            var subjectType = type.ToSubjectType();
            if (forceRefresh)
            {
                items[subjectType] = await Bangumi.Api.BangumiApi.BgmApi.Collections(subjectType);
            }
            return await Task.FromResult(items[subjectType]?.Collects ?? new List<Collection>());
        }

        public Task<bool> UpdateItemAsync(Collection item)
        {
            throw new NotImplementedException();
        }
    }
}
