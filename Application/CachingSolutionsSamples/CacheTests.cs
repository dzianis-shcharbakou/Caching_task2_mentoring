using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthwindLibrary;
using System.Linq;
using System.Threading;
using CachingSolutionsSamples.Employees;
using CachingSolutionsSamples.Suppliers;
using System.Data.Entity.Migrations;

namespace CachingSolutionsSamples
{
	[TestClass]
	public class CacheTests
	{
		[TestInitialize]
		public void Init()
		{

		}

		[TestMethod]
		public void CategoryMemoryCacheTest()
		{
			var categoryManager = new CategoriesManager(new CategoriesMemoryCache());

			for (var i = 0; i < 10; i++)
			{
				Console.WriteLine(categoryManager.GetCategories().Count());
				Thread.Sleep(100);
			}
		}

		[TestMethod]
		public void CategoryRedisCacheTest()
		{
			var categoryManager = new CategoriesManager(new CategoriesRedisCache("localhost"));

			categoryManager.CleanCategoryCache();

			for (var i = 0; i < 10; i++)
			{
				Console.WriteLine(categoryManager.GetCategories().Count());
				Thread.Sleep(100);
			}
		}

		[TestMethod]
		public void EmployeeMemoryCacheTest()
		{
			var employeeManager = new EmployeeManager(new EmployeeMemoryCache());

			for (var i = 0; i < 10; i++)
			{
				Console.WriteLine(employeeManager.GetEmployees().Count());
				Thread.Sleep(100);
			}
		}

		[TestMethod]
		public void EmployeeRedisCacheTest()
		{
			var employeeManager = new EmployeeManager(new EmployeeRedisCache("localhost"));

			employeeManager.CleanEmployeeCache();

			for (var i = 0; i < 10; i++)
			{
				Console.WriteLine(employeeManager.GetEmployees().Count());
				Thread.Sleep(100);
			}
		}

		[TestMethod]
		public void SupplierMemoryCacheTest()
		{
			#region Precondition Data
			var newSupplier = new Supplier()
			{
				Address = "newAddr",
				City = "newCity",
				CompanyName = "newCompanyName",
				ContactName = "newContactName",
				ContactTitle = "newContactTitle",
				Country = "newCountry",
				Fax = "newFax",
				HomePage = "newHomePage",
				Phone = "newPhone",
				PostalCode = "newPC",
				Region = "newRegion",
				SupplierID = 30
			};

			using (var dbContext = new Northwind())
			{
				dbContext.Configuration.LazyLoadingEnabled = false;
				dbContext.Configuration.ProxyCreationEnabled = false;
				dbContext.Suppliers.Add(newSupplier);
				dbContext.SaveChanges();
			}

			#endregion


			var employeeManager = new SupplierManager(new SupplierMemoryCache());

			for (var i = 0; i < 10; i++)
			{
				if (i == 5)
				{
					using (var dbContext = new Northwind())
					{
						dbContext.Configuration.LazyLoadingEnabled = false;
						dbContext.Configuration.ProxyCreationEnabled = false;
						if (dbContext.Suppliers.Any(x => x.CompanyName == newSupplier.CompanyName))
						{
							var entity = dbContext.Suppliers.First(x => x.CompanyName == newSupplier.CompanyName);
							dbContext.Suppliers.Remove(entity);
							dbContext.SaveChanges();
							Thread.Sleep(500);
						}
					}
				}

				Console.WriteLine(employeeManager.GetSuppliers().Count());
				Thread.Sleep(100);
			}
		}

		[TestMethod]
		public void SupplierRedisCacheTest()
		{
			var employeeManager = new SupplierManager(new SupplierRedisCache("localhost"));

			employeeManager.CleanEmployeeCache();

			#region Precondition Data
			var newSupplier = new Supplier()
			{
				Address = "newAddr",
				City = "newCity",
				CompanyName = "newCompanyName",
				ContactName = "newContactName",
				ContactTitle = "newContactTitle",
				Country = "newCountry",
				Fax = "newFax",
				HomePage = "newHomePage",
				Phone = "newPhone",
				PostalCode = "newPC",
				Region = "newRegion",
				SupplierID = 30
			};

			using (var dbContext = new Northwind())
			{
				dbContext.Configuration.LazyLoadingEnabled = false;
				dbContext.Configuration.ProxyCreationEnabled = false;
				dbContext.Suppliers.Add(newSupplier);
				dbContext.SaveChanges();
			}
			#endregion

			for (var i = 0; i < 10; i++)
			{
				if (i == 5)
				{
					using (var dbContext = new Northwind())
					{
						dbContext.Configuration.LazyLoadingEnabled = false;
						dbContext.Configuration.ProxyCreationEnabled = false;
						if (dbContext.Suppliers.Any(x => x.CompanyName == newSupplier.CompanyName))
						{
							var entity = dbContext.Suppliers.First(x => x.CompanyName == newSupplier.CompanyName);
							dbContext.Suppliers.Remove(entity);
							dbContext.SaveChanges();
							Thread.Sleep(500);
						}
					}
				}

				Console.WriteLine(employeeManager.GetSuppliers().Count());
				Thread.Sleep(100);
			}
		}
	}
}
