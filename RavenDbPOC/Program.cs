using System;
using System.Linq;
using NorthWind.Models;
using Raven.Client;
using Raven.Client.Documents;
using NorthWind;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace ContactsManager
{
  
    class Program
    {
        private IContactManager _manager;
        public Program(ContactManager manager)
        {
            _manager = manager;
        }
        static async Task Main(string[] args)
        {
            await new Program(new ContactManager()).RunAsync();
        }
      
        private async Task RunAsync()
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
                Console.WriteLine("M - Multi Map Index Verification");
                Console.WriteLine("X - Map/Reduce Index Verification");
                Console.WriteLine("L - Loading the Metadata");
                Console.WriteLine("P - Update the document using a PatchCommand");
                Console.WriteLine("B - Performing a batch operation");
                Console.WriteLine("A - Data Subscriptions");
                Console.WriteLine("S - Max Sales By Emplyee By Month");
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
                        await _manager.UpdateContactAsync();
                        break;
                    case ConsoleKey.D:
                        _manager.DeleteContact();
                        break;
                    case ConsoleKey.I:
                        _manager.SyncIndexCreation();
                        break;
                    case ConsoleKey.M:
                        _manager.MultiMapIndexVerification();
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
                    case ConsoleKey.X:
                        _manager.MapReduceIndexVerification();
                        break;
                    case ConsoleKey.S:
                        _manager.GetMaxSalesEmployeeByMonth();
                        break;
                    case ConsoleKey.L:
                        await _manager.GetMetaDataAsync();
                        break;
                    case ConsoleKey.P:
                        await _manager.UpdateDataByPatchAsync();
                        break;
                    case ConsoleKey.B:
                        await _manager.PerformingBatchOperationAsync();
                        break;
                    case ConsoleKey.A:
                        _manager.DataSubscription();
                        break;

                    default:
                        return;
                }

                Console.WriteLine("------------");
            }
        }
    }
}



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