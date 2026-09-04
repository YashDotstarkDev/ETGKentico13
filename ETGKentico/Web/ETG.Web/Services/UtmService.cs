using System.Web;
using ETG.Core.Http;
using ETG.Web.Models.Forms;
using ETG.Web.Services._Interfaces;
using Newtonsoft.Json;

namespace ETG.Web.Services
{
    public class UtmService : IUtmService
    {
        private readonly HttpRequest _httpRequest;
        public UtmService(IHttpRequest httpRequest)
        {
            _httpRequest = httpRequest.GetRequest();
        }

        public Utm GetUtmCookie()
        {
            return _httpRequest.Cookies["etgUtm"] != null
                ? JsonConvert.DeserializeObject<Utm>(_httpRequest.Cookies["etgUtm"].Value)
                : null;
        }
    }
}