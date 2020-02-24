using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace VXDesign.Store.DevTools.Core.Services.Cache
{
    public interface IMemoryCacheService
    {
        Task<T> Get<T>(string key, Func<Task<T>> initializer);
    }

    public class MemoryCacheService : IMemoryCacheService
    {
        private readonly MemoryCache cache;
        private readonly ConcurrentDictionary<object, SemaphoreSlim> locks;

        private MemoryCacheEntryOptions Options => new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(30));

        public MemoryCacheService()
        {
            cache = new MemoryCache(new MemoryCacheOptions());
            locks = new ConcurrentDictionary<object, SemaphoreSlim>();
        }

        public async Task<T> Get<T>(string key, Func<Task<T>> initializer)
        {
            if (!cache.TryGetValue(key, out T entry))
            {
                var @lock = locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));

                await @lock.WaitAsync();
                try
                {
                    if (!cache.TryGetValue(key, out entry))
                    {
                        entry = await initializer();
                        cache.Set(key, entry, Options);
                    }
                }
                finally
                {
                    @lock.Release();
                }
            }

            return entry;
        }
    }
}