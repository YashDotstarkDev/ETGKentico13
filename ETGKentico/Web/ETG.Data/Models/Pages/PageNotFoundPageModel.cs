using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Devotion.Automapper.Common;
using ETG.Data.Article.Models;
using ETG.Data.Models.Base;
using ETG.Data.Models.Common;
using ETG.Data.Tour.Models;

namespace ETG.Data.Models.Pages
{
    public class PageNotFoundPageModel : BasePageModel, IDataModel
    {

        public ContactModel Contact { get; set; }
        public TourListingModel Tours { get; set; }
        public List<ArticleModel> Articles { get; set; }
    }
}
