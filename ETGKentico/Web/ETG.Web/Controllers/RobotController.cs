using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ETG.Data.Repositories._Interfaces;

namespace ETG.Web.Controllers
{
    public class RobotController : Controller
    {
        private readonly IRobotRepository _robotRepository;
        public RobotController(IRobotRepository robotRepository)
        {
            _robotRepository = robotRepository;
        }

        public ContentResult Index()
        {
            var result = new ContentResult();
            result.Content = _robotRepository.GetRobotsTxtContents();
            result.ContentType = "text/plain";
            return result;
        }
    }
}