using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETG.Data.Search
{
    public interface ISearchConfiguration
    {
        string ApplicationId { get; }
        string APIKey { get; }
        string IndexTypeAhead { get; }
        string IndexTour { get; }
        string IndexArticle { get; }
        string IndexContents { get; }
    }
}
