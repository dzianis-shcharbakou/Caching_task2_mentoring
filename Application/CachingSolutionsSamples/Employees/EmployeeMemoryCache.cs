using NorthwindLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace CachingSolutionsSamples.Employees
{
    public class EmployeeMemoryCache : IEntityCache<Employee>
	{
		ObjectCache cache = MemoryCache.Default;
		string prefix = "Cache_Employees";

		public void Delete(string forUser)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Employee> Get(string forUser)
		{
			return (IEnumerable<Employee>)cache.Get(prefix + forUser);
		}

		public void Set(string forUser, IEnumerable<Employee> entities)
		{
			cache.Set(prefix + forUser, entities, new DateTimeOffset(DateTime.UtcNow.AddMilliseconds(300)));
		}
	}
}
