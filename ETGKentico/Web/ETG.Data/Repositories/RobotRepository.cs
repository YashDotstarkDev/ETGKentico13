using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETG.Core.PageTypes.Providers;
using ETG.Data.Repositories._Interfaces;

namespace ETG.Data.Repositories
{
    public class RobotRepository : IRobotRepository
    {
        public string GetRobotsTxtContents()
        {
            return RobotProvider.GetRobots().OnCurrentSite().TopN(1).Select(a=>a.RobotContent).FirstOrDefault();
        }
    }
}
