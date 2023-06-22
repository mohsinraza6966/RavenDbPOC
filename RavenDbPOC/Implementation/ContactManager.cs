﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactsManager;
using NorthWind.Models;
using Raven.Client.Documents;
using RavenDbPOC.Utility;

namespace NorthWind
{
    public class ContactManager : IContactManager
    {
        public void CreateContact()
        {
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                Console.WriteLine("Name: ");
                var name = Console.ReadLine();

                Console.WriteLine("Email: ");
                var email = Console.ReadLine();

                var contact = new Contact
                {
                    Name = name,
                    Email = email
                };

                session.Store(contact);

                Console.WriteLine($"New Contact ID {contact.Id}");

                session.SaveChanges();
            }
        }

        public void DeleteContact()
        {
            Console.WriteLine("Enter the contact id: ");
            var id = Console.ReadLine();

            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                var contact = session.Load<Contact>(id);

                if (contact == null)
                {
                    Console.WriteLine("Contact not found.");
                    return;
                }

                session.Delete(contact);
                session.SaveChanges();
            }
        }

        public void QueryAllContacts()
        {
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                var contacts = session.Query<Contact>().ToList();

                foreach (var contact in contacts)
                {
                    Console.WriteLine($"{contact.Id} - {contact.Name} - {contact.Email}");
                }

                Console.WriteLine($"{contacts.Count} contacts found.");
            }
        }

        public void RetrieveContact()
        {
            Console.WriteLine("Enter the contact id: ");
            var id = Console.ReadLine();

            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                var contact = session.Load<Contact>(id);

                if (contact == null)
                {
                    Console.WriteLine("Contact not found.");
                    return;
                }

                Console.WriteLine($"Name: {contact.Name}");
                Console.WriteLine($"Email: {contact.Email}");
            }
        }

        public async Task UpdateContactAsync()
        {
            Console.WriteLine("Enter the contact id: ");
            var id = Console.ReadLine();

            using (var session = DocumentStoreHolder.Store.OpenAsyncSession())
            {
                var contact = await session.LoadAsync<Contact>(id);

                if (contact == null)
                {
                    Console.WriteLine("Contact not found.");
                    
                }

                Console.WriteLine($"Actual name: {contact.Name}");
                Console.WriteLine("New name: ");
                contact.Name = Console.ReadLine();

                Console.WriteLine($"Actual email: {contact.Email}");
                Console.WriteLine("New email address: ");
                contact.Email = Console.ReadLine();

                await session.SaveChangesAsync();
            }
        }
        public void GetSortedOrderIds()
        {
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                var ordersIds = (
                    from order in session.Query<Order>()
                    where order.Company == "companies/1-A"
                    orderby order.OrderedAt
                    select order.Id
                    ).ToList();

                foreach (var id in ordersIds)
                {
                    Console.WriteLine(id);
                }
            }
        }

        public void MapReduceIndexVerification() {
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                var results = session
                    .Query<Products_ByCategory.Result, Products_ByCategory>()
                    .Include(x => x.Category)
                    .ToList();

                foreach (var result in results)
                {
                    var category = session.Load<Category>(result.Category);
                    Console.WriteLine($"Name:  {category.Name} has {result.Count} items.");
                }
            }
        }
        public void SyncIndexCreation()
        {
            //var store = DocumentStoreHolder.Store;
            //new Employees_ByFirstAndLastName().Execute(store);
        }
        public void VerifyIndexCreation()
        {
            try
            {
                using (var session = DocumentStoreHolder.Store.OpenSession())
                {
                    var results = session
                        .Query<Employee, Employees_ByFirstAndLastName>()
                        .Where(x => x.FirstName == "Robert")
                        .ToList();

                    foreach (var employee in results)
                    {
                        Console.WriteLine($"{employee.LastName}, {employee.FirstName}");
                    }
                }


            }
            catch (Exception ex)
            {

                Console.WriteLine("ex.Message: " + ex.Message);
                Console.WriteLine("ex.ToString: " + ex.ToString());
                Console.WriteLine("ex.InnerException: " + ex.InnerException);
            }
        }

        public void MultiMapIndexVerification()
        {
            try
            {

                Console.Title = "Multi-map sample";
                using (var session = DocumentStoreHolder.Store.OpenSession())
                {

                    Console.Write("\nSearch terms: ");
                    var searchTerms = Console.ReadLine();

                    foreach (var result in IndexUtility.Search(session, searchTerms))
                    {
                        Console.WriteLine($"{result.SourceId}\t{result.Type}\t{result.Name}");
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("ex.Message: " + ex.Message);
                Console.WriteLine("ex.ToString: " + ex.ToString());
                Console.WriteLine("ex.InnerException: " + ex.InnerException);
            }
        }

        public void GetMaxSalesEmployeeByMonth()
        {
            try
            {

                using (var session = DocumentStoreHolder.Store.OpenSession())
                {
                    var query = session
                        .Query<Employees_SalesPerMonth.Result, Employees_SalesPerMonth>()
                        .Include(x => x.Employee);

                    var results = (
                        from result in query
                        where result.Month == "1998-03"
                        orderby result.TotalSales descending
                        select result
                        ).ToList();

                    foreach (var result in results)
                    {
                        var employee = session.Load<Employee>(result.Employee);
                        Console.WriteLine(
                            $"{employee.FirstName} {employee.LastName}"
                            + $" made {result.TotalSales} sales.");
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("ex.Message: " + ex.Message);
                Console.WriteLine("ex.ToString: " + ex.ToString());
                Console.WriteLine("ex.InnerException: " + ex.InnerException);
            }
        }
    }
}
