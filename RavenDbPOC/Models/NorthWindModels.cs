using Raven.Client.Documents.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.Models
{
	public class Category
	{
		public string Name { get; set; }
	}

	public class Product
	{
		public string Category { get; set; }
	}
	public  class Contact
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
    public class Order
    {
		public string Employee { get; }
		public string Id { get; set; }
        public string Company { get; set; }
        public DateTimeOffset OrderedAt { get; set; }
    }

	///
	public class Location
	{
		public float Latitude { get; set; }
		public float Longitude { get; set; }
	}

	public class Address
	{
		public string City { get; set; }
		public string Country { get; set; }
		public string Line1 { get; set; }
		public object Line2 { get; set; }
		public Location Location { get; set; }
		public int PostalCode { get; set; }
		public string Region { get; set; }
	}

	public class Employee
	{
		public string Id { get; set; }
		public string FirstName { get; set; }
		
		public string LastName { get; set; }
		
	}
	public class Company
	{
		public string Id { get; set; }

		public string Name { get; set; }

        public Address Address { get; set; }
        public Contact Contact { get; set; }
	}

	public class Supplier
	{
		public string Id { get; set; }
		public Contact Contact { get; set; }
	}
	public class Employees_ByFirstAndLastName : AbstractIndexCreationTask<Employee>
	{
		public Employees_ByFirstAndLastName()
		{
			Map = (employees) =>
				from employee in employees
				select new
				{
					FirstName = employee.FirstName,
					LastName = employee.LastName
				};
		}
	}

	public class PeopleSearchResult
	{
		public string SourceId { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }
	}
	public class People_Search : AbstractMultiMapIndexCreationTask<PeopleSearchResult>
	{
		public People_Search()
		{
			AddMap<Company>(companies =>
				from company in companies
				select new PeopleSearchResult
				{
					SourceId = company.Id,
					Name = company.Contact.Name,
					Type = "Company's contact"
				}
				);

			AddMap<Supplier>(suppliers =>
				from supplier in suppliers
				select new PeopleSearchResult
				{
					SourceId = supplier.Id,
					Name = supplier.Contact.Name,
					Type = "Supplier's contact"
				}
				);

			AddMap<Employee>(employees =>
				from employee in employees
				select new PeopleSearchResult
				{
					SourceId = employee.Id,
					Name = $"{employee.FirstName} {employee.LastName}",
					Type = "Employee"
				}
				);

			Index(entry => entry.Name, FieldIndexing.Search);

			Store(entry => entry.SourceId, FieldStorage.Yes);
			Store(entry => entry.Name, FieldStorage.Yes);
			Store(entry => entry.Type, FieldStorage.Yes);
		}
	}

	public class Products_ByCategory : AbstractIndexCreationTask<Product, Products_ByCategory.Result>
	{
		public class Result
		{
			public string Category { get; set; }
			public int Count { get; set; }
		}

		public Products_ByCategory()
		{
			Map = products =>
				from product in products
				select new
				{
					Category = product.Category,
					Count = 1
				};

			Reduce = results =>
				from result in results
				group result by result.Category into g
				select new
				{
					Category = g.Key,
					Count = g.Sum(x => x.Count)
				};
		}
	}

	public class Employees_SalesPerMonth :
	AbstractIndexCreationTask<Order, Employees_SalesPerMonth.Result>
	{
		public class Result
		{
			public string Employee { get; set; }
			public string Month { get; set; }
			public int TotalSales { get; set; }
		}

		public Employees_SalesPerMonth()
		{
			Map = orders =>
				from order in orders
				select new
				{
					order.Employee,
					Month = order.OrderedAt.ToString("yyyy-MM"),
					TotalSales = 1
				};

			Reduce = results =>
				from result in results
				group result by new
				{
					result.Employee,
					result.Month
				}
				into g
				select new
				{
					g.Key.Employee,
					g.Key.Month,
					TotalSales = g.Sum(x => x.TotalSales)
				};
		}

		public class Products_ByCategoryLookUp :AbstractIndexCreationTask<Product, Products_ByCategoryLookUp.Result>
		{
			public class Result
			{
				public string Category { get; set; }
				public int Count { get; set; }
			}

			public Products_ByCategoryLookUp()
			{
				Map = products =>
					from product in products
					let categoryName = LoadDocument<Category>(product.Category).Name
					select new
					{
						Category = categoryName,
						Count = 1
					};

				Reduce = results =>
					from result in results
					group result by result.Category into g
					select new
					{
						Category = g.Key,
						Count = g.Sum(x => x.Count)
					};
			}
		}
	}
}
