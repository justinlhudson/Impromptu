using System;
using System.Runtime.Caching;

namespace Impromptu.Collection
{
	/*
		public class ConcurrentMemoryCache
		{
			private readonly MemoryCache _cache;
			private readonly object _lock = new object();

			public ConcurrentMemoryCache(string name = null)
			{
				if (name == null)
					name = Guid.NewGuid().ToString();
				_cache = new MemoryCache(name);
			}

			public object Get(string key)
			{
				return AddOrGetExisting(key, () => Init(key));
			}

			public T AddOrGetExisting<T>(string key, Func<T> value)
			{
				lock (_lock)
				{
					var newValue = new Lazy<T>(value);
					var oldValue = _cache.AddOrGetExisting(key, newValue, new CacheItemPolicy()) as Lazy<T>;
					try
					{
						return (oldValue ?? newValue).Value;
					}
					catch
					{
						_cache.Remove(key); // remove 
						throw;
					}
				}
			}

			private static object Init(string key)
			{
				// Do something expensive to initialize item
				return new { Value = key };
			}
		}
	*/
}