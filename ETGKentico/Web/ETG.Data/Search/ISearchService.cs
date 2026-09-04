using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETG.Data.Search.Models;

namespace ETG.Data.Search
{
    public interface ISearchService
    {
        TypeAheadSearchResult TypeAheadSearch(string keywords);
    }
}
