using NorthwindLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace CachingSolutionsSamples.Suppliers
{
	public class SupplierMemoryCache : IEntityCache<Supplier>
	{
		string prefix = "Cache_Suppliers";

		private static MemoryCache cache;

		public SupplierMemoryCache()
		{
			cache = MemoryCache.Default;
		}

		public void Delete(string key)
		{
			cache.Remove(key);
		}

		public IEnumerable<Supplier> Get(string forUser)
		{
			return (IEnumerable<Supplier>)cache.Get(prefix + forUser);
		}

		public void Set(string forUser, IEnumerable<Supplier> entities)
		{
			
			using (var dbContext = new Northwind())
			{
				dbContext.Database.Connection.Open();
				SqlDependency.Start(dbContext.Database.Connection.ConnectionString);
				using (SqlCommand sqlComm = new SqlCommand("select [SupplierID] from dbo.Suppliers", (SqlConnection)dbContext.Database.Connection))
				{
					var policy = new CacheItemPolicy();

					SqlDependency dependency = new SqlDependency(sqlComm
							);

					SqlChangeMonitor monitor = new SqlChangeMonitor(dependency);

					policy.ChangeMonitors.Add(monitor);

					cache.Set(prefix + forUser, entities, policy);
					sqlComm.ExecuteNonQuery();

					
				}
			}

		}
	}
}
