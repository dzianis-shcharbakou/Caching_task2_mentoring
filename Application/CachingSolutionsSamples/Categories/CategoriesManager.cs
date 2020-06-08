using NorthwindLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CachingSolutionsSamples
{
	public class CategoriesManager
	{
		private IEntityCache<Category> cache;

		public CategoriesManager(IEntityCache<Category> cache)
		{
			this.cache = cache;
		}

		public IEnumerable<Category> GetCategories()
		{
			Console.WriteLine("Get Categories");

			var user = Thread.CurrentPrincipal.Identity.Name;
			var entities = cache.Get(user);

			if (entities == null)
			{
				Console.WriteLine("From DB");

				using (var dbContext = new Northwind())
				{
					dbContext.Configuration.LazyLoadingEnabled = false;
					dbContext.Configuration.ProxyCreationEnabled = false;
					entities = dbContext.Categories.ToList();
					cache.Set(user, entities);
				}
			}

			return entities;
		}

		public void CleanCategoryCache()
		{
			var user = Thread.CurrentPrincipal.Identity.Name;
			this.cache.Delete(user);
		}
	}
}
