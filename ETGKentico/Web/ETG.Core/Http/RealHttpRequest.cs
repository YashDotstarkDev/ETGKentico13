using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ETG.Core.Http
{
    public class RealHttpRequest : IHttpRequest
    {
        public HttpRequest GetRequest()
        {
            return HttpContext.Current.Request;
        }
    }
}
