using NorthWind.Models;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenDbPOC.Utility
{
    public static class IndexUtility
    {
        public static IEnumerable<PeopleSearchResult> Search(IDocumentSession session,string searchTerms)
        {
            var results = session.Query<PeopleSearchResult, People_Search>()
                .Search(
                    r => r.Name,
                    searchTerms
                )
                .ProjectInto<PeopleSearchResult>()
                .ToList();

            return results;
        }
    }
}
