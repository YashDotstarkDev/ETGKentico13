using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETG.Web.Brochure.Services
{
    public interface IBrochureService
    {
        string GetBrochureHtmlLinks(string guids);
    }
}
