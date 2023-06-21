using Raven.Client.Documents.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.Models
{
    public  class Contact
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
    public class Order
    {
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
}
