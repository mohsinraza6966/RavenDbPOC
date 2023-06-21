//using Raven.Client.Documents;
//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace RavenDbPOC
//{
//    public class DocumentStoreService : IDocumentStore
//    {
//        public DocumentStore DocumentStore { get; set; }
//        public void Intiallize()
//        {
//            this.DocumentStore = new DocumentStore
//            {
//                Urls = new[] { ConfigurationManager.AppSettings.Get("BaseUrl") },
//                Database = ConfigurationManager.AppSettings.Get("DbName")
//            };

//            this.DocumentStore.Initialize();
//        }
        

//    }

//    public static class DocumentStoreHolder
//    {
//        private static readonly Lazy<IDocumentStore> LazyStore =
//            new Lazy<IDocumentStore>(() =>
//            {
//                var dstore = new DocumentStoreService();
//                dstore.Intiallize();
//                var store = dstore.DocumentStore;
//                var store = new DocumentStore
//                {
//                    Urls = new[] { "http://localhost:8080" },
//                    Database = "Northwind"
//                };

//                return dstore.DocumentStore;
//            });

//        public static IDocumentStore Store =>
//            LazyStore.Value;
//    }
//}
