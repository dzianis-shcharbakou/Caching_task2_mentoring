using NorthwindLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CachingSolutionsSamples.Suppliers
{
    public class SupplierManager
    {
		private IEntityCache<Supplier> cache;

		public SupplierManager(IEntityCache<Supplier> cache)
		{
			this.cache = cache;
		}

		public IEnumerable<Supplier> GetSuppliers()
		{
			Console.WriteLine("Get Supplier");

			var user = Thread.CurrentPrincipal.Identity.Name;
			var entities = cache.Get(user);

			if (entities == null)
			{
				Console.WriteLine("From DB");

				using (var dbContext = new Northwind())
				{
					dbContext.Configuration.LazyLoadingEnabled = false;
					dbContext.Configuration.ProxyCreationEnabled = false;
					entities = dbContext.Suppliers.ToList();
					cache.Set(user, entities);
				}
			}

			return entities;
		}

		public void CleanEmployeeCache()
		{
			var user = Thread.CurrentPrincipal.Identity.Name;
			this.cache.Delete(user);
		}
	}
}
