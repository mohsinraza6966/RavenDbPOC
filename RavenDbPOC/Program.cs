using System;
using System.Linq;
using NorthWind.Models;
using Raven.Client;
using Raven.Client.Documents;
using NorthWind;
using Microsoft.Extensions.DependencyInjection;

namespace ContactsManager
{
  
    class Program
    {
        private IContactManager _manager;
        public Program(ContactManager manager)
        {
            _manager = manager;
        }
        static void Main(string[] args)
        {
            new Program(new ContactManager()).Run();
        }
      
        private void Run()
        {
            //_manager = new ContactManager();
            while (true)
            {
                Console.WriteLine("Please, press:");
                Console.WriteLine("C - Create");
                Console.WriteLine("R - Retrieve");
                Console.WriteLine("U - Update");
                Console.WriteLine("D - Delete");
                Console.WriteLine("I - Sync Index To Server");
                Console.WriteLine("O - Sorted OrderId's");
                Console.WriteLine("V - Verify Index Creation");
                Console.WriteLine("Q - Query all contacts (limit to 128 items)");

                var input = Console.ReadKey();

                Console.WriteLine("\n------------");

                switch (input.Key)
                {
                    case ConsoleKey.C:
                        _manager.CreateContact();
                        break;
                    case ConsoleKey.R:
                        _manager.RetrieveContact();
                        break;
                    case ConsoleKey.U:
                        _manager.UpdateContact();
                        break;
                    case ConsoleKey.D:
                        _manager.DeleteContact();
                        break;
                    case ConsoleKey.I:
                        _manager.SyncIndexCreation();
                        break;
                    case ConsoleKey.Q:
                        _manager.QueryAllContacts();
                        break;
                    case ConsoleKey.O:
                        _manager.GetSortedOrderIds();
                        break;
                    case ConsoleKey.V:
                        _manager.VerifyIndexCreation();
                        break;
                    default:
                        return;
                }

                Console.WriteLine("------------");
            }
        }
    }
}

//        private void CreateContact()
//        {
//            using (var session = DocumentStoreHolder.Store.OpenSession())
//            {
//                Console.WriteLine("Name: ");
//                var name = Console.ReadLine();

//                Console.WriteLine("Email: ");
//                var email = Console.ReadLine();

//                var contact = new Contact
//                {
//                    Name = name,
//                    Email = email
//                };

//                session.Store(contact);

//                Console.WriteLine($"New Contact ID {contact.Id}");

//                session.SaveChanges();
//            }
//        }
//        private void RetrieveContact()
//        {
//            Console.WriteLine("Enter the contact id: ");
//            var id = Console.ReadLine();

//            using (var session = DocumentStoreHolder.Store.OpenSession())
//            {
//                var contact = session.Load<Contact>(id);

//                if (contact == null)
//                {
//                    Console.WriteLine("Contact not found.");
//                    return;
//                }

//                Console.WriteLine($"Name: {contact.Name}");
//                Console.WriteLine($"Email: {contact.Email}");
//            }
//        }
//        private void UpdateContact()
//        {
//            Console.WriteLine("Enter the contact id: ");
//            var id = Console.ReadLine();

//            using (var session = DocumentStoreHolder.Store.OpenSession())
//            {
//                var contact = session.Load<Contact>(id);

//                if (contact == null)
//                {
//                    Console.WriteLine("Contact not found.");
//                    return;
//                }

//                Console.WriteLine($"Actual name: {contact.Name}");
//                Console.WriteLine("New name: ");
//                contact.Name = Console.ReadLine();

//                Console.WriteLine($"Actual email: {contact.Email}");
//                Console.WriteLine("New email address: ");
//                contact.Email = Console.ReadLine();

//                session.SaveChanges();
//            }
//        }
//        private void DeleteContact()
//        {
//            Console.WriteLine("Enter the contact id: ");
//            var id = Console.ReadLine();

//            using (var session = DocumentStoreHolder.Store.OpenSession())
//            {
//                var contact = session.Load<Contact>(id);

//                if (contact == null)
//                {
//                    Console.WriteLine("Contact not found.");
//                    return;
//                }

//                session.Delete(contact);
//                session.SaveChanges();
//            }
//        }
//        private void QueryAllContacts()
//        {
//            using (var session = DocumentStoreHolder.Store.OpenSession())
//            {
//                var contacts = session.Query<Contact>().ToList();

//                foreach (var contact in contacts)
//                {
//                    Console.WriteLine($"{contact.Id} - {contact.Name} - {contact.Email}");
//                }

//                Console.WriteLine($"{contacts.Count} contacts found.");
//            }
//        }
//    }
//}






/*

using System;
using Raven.Client.Documents;
using StackExchange.Redis;

namespace RavenDbPOC
{
    class Program
    {

        static void Main(string[] args)
        {

            //DocumentStoreService storeService = new DocumentStoreService();
            //storeService.Intiallize();

            //using (var session = storeService.DocumentStore.OpenSession())
            //{
            //    //var p = session.Load<ProductPartial>("products/1-A");
            //    var p2 = session.Load<Product>("products/1-A");
            //    var p = session.Load<Product>("products/1-A");
            //    var c = session.Load<Category>(p.Category);
            //    //System.Console.WriteLine(p.Name);


            //}

            //using (var session = DocumentStoreHolder.Store.OpenSession())
            //{
            //    var p = session.Load<Product>("products/1-A");
            //    System.Console.WriteLine(p.Name);
            //}


            Console.WriteLine("Hello World!");
        }
    }
}
*/