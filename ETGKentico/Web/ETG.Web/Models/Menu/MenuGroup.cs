using ETG.Data.Models.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETG.Web.Models.Menu
{
    public class MenuGroup
    {
        public LinkViewModel Link;
        public List<LinkViewModel> ChildLinks;
    }
}