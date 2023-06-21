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
		public Address Address { get; set; }
		public DateTimeOffset Birthday { get; set; }
		public int Extension { get; set; }
		public string FirstName { get; set; }
		public DateTimeOffset HiredAt { get; set; }
		public string HomePhone { get; set; }
		public string LastName { get; set; }
		public List<string> Notes { get; set; }
		public string ReportsTo { get; set; }
		public List<string> Territories { get; set; }
		public string Title { get; set; }
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
}
