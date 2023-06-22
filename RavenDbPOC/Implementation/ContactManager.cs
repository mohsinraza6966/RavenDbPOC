
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ContactsManager;
using NorthWind.Models;
using Raven.Client.Documents;
using Raven.Client.Documents.Commands;
using Raven.Client.Documents.Commands.Batches;
using Raven.Client.Documents.Operations;
using RavenDbPOC.Utility;
using Sparrow.Json;

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

        public void MapReduceIndexVerification()
        {
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

                // request Name, City and Country 
                //// for all entities from 'Companies' collection
                //using (var session = DocumentStoreHolder.Store.OpenSession())
                //{


                //    var results = session
                //        .Query<Company>().Statistics(out var stats)
                //        .Select(x => new
                //        {
                //            Name = x.Name,
                //            City = x.Address.City,
                //            Country = x.Address.Country
                //        })
                //        .ToList();
                //    Console.WriteLine( JsonSerializer.Serialize<object>(results));
                //}

                using (var session = DocumentStoreHolder.Store.OpenSession())
                {
                    var query = session.Query<Order>()
                                       .Customize(q =>
                                                     q.WaitForNonStaleResults(TimeSpan.FromSeconds(5))
                                        );

                    var orders = (
                        from order in query
                        where order.Company == "companies/1"
                        orderby order.OrderedAt
                        select order
                        )
                        .ToList();
                }
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

        public async Task GetMetaDataAsync()
        {
            //using (var session = DocumentStoreHolder.Store.OpenAsyncSession())
            //{
            //    var product = await session.LoadAsync<Product>("products/1-A");
            //    var metadata = session.Advanced.GetMetadataFor(product);

            //    metadata["last-modified-by"] = "Mohsin Raza";
            //    await session.SaveChangesAsync();
            //}

            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                var command = new GetDocumentsCommand(
                    "products/1-a", null, metadataOnly: true);
                session.Advanced.RequestExecutor.Execute(
                    command, session.Advanced.Context);
                var result = (BlittableJsonReaderObject)command.Result.Results[0];
                var metadata = (BlittableJsonReaderObject)result["@metadata"];

                foreach (var propertyName in metadata.GetPropertyNames())
                {
                    metadata.TryGet<object>(propertyName, out var value);
                    Console.WriteLine($"{propertyName}: {value}");
                }
            }
        }

        public async Task UpdateDataByPatchAsync()
        {
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                session.Advanced.Defer(new PatchCommandData(
                    id: "orders/816-A",
                    changeVector: null,
                    patch: new PatchRequest
                    {
                        Script = "this.Lines.push(args.NewLine)",
                        Values =
                        {
                            {
                                "NewLine", new 
                                {
                                    Product = "products/1-a",
                                    ProductName = "Chai",
                                    PricePerUnit=18M,
                                    Quantity=1,
                                    Discount=0
                                }
                            }
                        }

                    },
                    patchIfMissing: null));

                session.SaveChanges();
            }
        }
    }
}
