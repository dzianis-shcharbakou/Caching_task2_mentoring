using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NorthwindLibrary;
using System.Runtime.Caching;

namespace CachingSolutionsSamples
{
	internal class CategoriesMemoryCache : IEntityCache<Category>
	{
		ObjectCache cache = MemoryCache.Default;
		string prefix  = "Cache_Categories";

		public void Delete(string forUser)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Category> Get(string forUser)
		{
			return (IEnumerable<Category>) cache.Get(prefix + forUser);
		}

		public void Set(string forUser, IEnumerable<Category> entities)
		{
			cache.Set(prefix + forUser, entities, new DateTimeOffset(DateTime.UtcNow.AddMilliseconds(300)));
		}
	}
}
