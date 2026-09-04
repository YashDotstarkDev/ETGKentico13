using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ETG.Data.Serialization
{
    public class XmlResult<T> : ActionResult
    {
        private readonly T data;
     
        public XmlResult(T data)
        {
            this.data = data;
        }



        public override void ExecuteResult(ControllerContext context)
        {
           
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = "text/xml";
            response.ContentEncoding = Encoding.UTF8;
            response.BufferOutput = false;
            new XmlSerializer().SerializeToStream<T>(this.data, response.OutputStream);
        }
    }
}
