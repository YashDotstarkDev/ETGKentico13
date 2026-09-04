using Castle.Core.Internal;
using Devotion.Web.Base.Extensions;
using ETG.Core.Http;

namespace ETG.Data.Services
{
    public class ShareLinksService : IShareLinksService
    {
        private readonly System.Web.HttpRequest _request;

        public ShareLinksService(IHttpRequest httpRequest)
        {
            _request = httpRequest.GetRequest();
        }

        private string HttpDomain => $"{(_request.IsSecureConnection ? "https" : "http")}://{_request.Url.Host}";

        private string EncodedCurrentUrl => $"{HttpDomain}{_request.RawUrl}";

        public string GetFacebookShareLink()
        {
            return $"https://www.facebook.com/sharer/sharer.php?u={EncodedCurrentUrl}";
        }

        public string GetTwitterShareLink()
        {
            return $"https://twitter.com/intent/tweet?text={EncodedCurrentUrl}";
        }

        public string GetLinkedInShareLink()
        {
            return $"https://www.linkedin.com/shareArticle?mini=true&url={EncodedCurrentUrl}";
        }

        public string GetPinterestShareLink(string imagePath, string description = "")
        {
            return imagePath.IsNullOrEmpty()
                ? string.Empty
                : $"http://pinterest.com/pin/create/button/?url={EncodedCurrentUrl}&media={HttpDomain}{imagePath.RemoveTilde()}&description={description}";
        }

        public string GetCopyLink()
        {
            return EncodedCurrentUrl;
        }
    }
}