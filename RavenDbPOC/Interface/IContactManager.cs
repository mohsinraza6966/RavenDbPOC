using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind
{
    interface IContactManager
    {
        public void CreateContact();
        public void RetrieveContact();
        
        public void DeleteContact();
        public void QueryAllContacts();
        public void GetSortedOrderIds();
        public void SyncIndexCreation();
        public void VerifyIndexCreation();
        public void MultiMapIndexVerification();
        public void MapReduceIndexVerification();

        public void GetMaxSalesEmployeeByMonth();
        //public void UpdateContact();
        public  Task UpdateContactAsync();

        public Task GetMetaDataAsync();
    }
}
