//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace RavenDbPOC
//{
//    public class ProductPartial
//    {
//        public string Id { get; set; }
//        public string Name { get; set; }
//    }

//    public class Category
//    {
//        public string Description { get; set; }
//        public string Name { get; set; }
//    }
//    public class Product
//    {
//        public string Name { get; set; }
//        public string Supplier { get; set; }
//        public string Category { get; set; }
//        public string QuantityPerUnit { get; set; }
//        public float PricePerUnit { get; set; }
//        public int UnitsInStock { get; set; }
//        public int UnitsOnOrder { get; set; }
//        public bool Discontinued { get; set; }
//        public int ReorderLevel { get; set; }
//    }

//	public class Location
//	{
//		public float Latitude { get; set; }
//		public float Longitude { get; set; }
//	}

//	public class Address
//	{
//		public string City { get; set; }
//		public string Country { get; set; }
//		public string Line1 { get; set; }
//		public object Line2 { get; set; }
//		public Location Location { get; set; }
//		public DateTimeOffset PostalCode { get; set; }
//		public object Region { get; set; }
//	}

//	public class Contact
//	{
//		public string Name { get; set; }
//		public string Title { get; set; }
//	}

//	public class Order
//	{
//		public Address Address { get; set; }
//		public Contact Contact { get; set; }
//		public string ExternalId { get; set; }
//		public string Fax { get; set; }
//		public string Name { get; set; }
//		public string Phone { get; set; }
//	}
//}
