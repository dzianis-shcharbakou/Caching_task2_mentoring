using NorthwindLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CachingSolutionsSamples.Employees
{
    public class EmployeeManager
    {
		private IEntityCache<Employee> cache;

		public EmployeeManager(IEntityCache<Employee> cache)
		{
			this.cache = cache;
		}

		public IEnumerable<Employee> GetEmployees()
		{
			Console.WriteLine("Get Employees");

			var user = Thread.CurrentPrincipal.Identity.Name;
			var entities = cache.Get(user);

			if (entities == null)
			{
				Console.WriteLine("From DB");

				using (var dbContext = new Northwind())
				{
					dbContext.Configuration.LazyLoadingEnabled = false;
					dbContext.Configuration.ProxyCreationEnabled = false;
					entities = dbContext.Employees.ToList();
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
