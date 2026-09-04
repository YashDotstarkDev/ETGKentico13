using System;
using System.Configuration;
using System.Text;
using Castle.Core.Internal;
using CMS.Helpers;

namespace ETG.Core.Extensions
{
    public static class ImgixExtensions
    {
        private static bool ImgixEnabled()
        {
            return !ImgixDomain().IsNullOrEmpty();
        }

        private static string ImgixDomain()
        {
            if (System.Web.HttpContext.Current == null)
            {
                return ConfigurationManager.AppSettings["ImgixDomain"];
            }
            var imgixDomain = System.Web.HttpContext.Current.Application["ImgixDomain"];

            if (imgixDomain != null && !imgixDomain.ToString().IsNullOrEmpty())
            {
                return imgixDomain.ToString();
            }

            System.Web.HttpContext.Current.Application["ImgixDomain"] = ConfigurationManager.AppSettings["ImgixDomain"];
            return ConfigurationManager.AppSettings["ImgixDomain"];
        }

        private static string RemoveParams(this string url)
        {
            return url.Contains("?") ? url.Split('?')[0] : url;
        }

        private static Uri GetUri(string url)
        {
            return new Uri(url, UriKind.RelativeOrAbsolute);
        }

        private static string CombineUrl(params string[] urls)
        {
            return string.Join("", urls);
        }

        private static string AddAutoFormat(this string url)
        {
            if (url.IsNullOrEmpty())
            {
                return string.Empty;
            }

            if (url.IndexOf("?") > -1)
            {
                return $"{url}&auto=format";
            }

            return $"{url}?auto=format";

        }

        private static string AddLowResWidth(this string url)
        {
            if (url.IsNullOrEmpty())
            {
                return string.Empty;
            }

            if (url.IndexOf("?") > -1)
            {
                return $"{url}&w=3";
            }

            return $"{url}?w=3";

        }

        private static string AddResizerHeight(this string url, int height)
        {
            return string.IsNullOrEmpty(url)
                ? string.Empty
                : URLHelper.AddParameterToUrl(url, "h", height.ToString());
        }

        private static string AddResizerWidth(this string url, int width)
        {
            return string.IsNullOrEmpty(url)
                ? string.Empty
                : (width == 0 ? URLHelper.AddParameterToUrl(url, "w", "{width}") : URLHelper.AddParameterToUrl(url, "w", width.ToString()));
        }

        public static string Imgixify(this string url, bool removeParams = true)
        {
            if (string.IsNullOrEmpty(url))
            {
                return string.Empty;
            }

            if (url.StartsWith("~"))
            {
                url = url.TrimStart('~');
            }

            if (!ImgixEnabled())
            {
                return url;
            }

            
            if (removeParams)
            {
                url = url.RemoveParams();
            }

            //required by azure blob
            url = url.Replace("/ETG/media", "/etg/media");

            try
            {
                var imgixUrl = ImgixDomain();
                var orignalUri = GetUri(url);

                return CombineUrl(imgixUrl, !orignalUri.IsAbsoluteUri ? url.TrimStart('~') : orignalUri.PathAndQuery).AddAutoFormat();
            }
#if DEBUG
            catch (Exception ex)
# else
            catch 
#endif
            {
                return url;
            }
        }

        public static string ImgixifyNoParameter(this string url, bool removeParams = true)
        {
            if (string.IsNullOrEmpty(url))
            {
                return string.Empty;
            }

            if (url.StartsWith("~"))
            {
                url = url.TrimStart('~');
            }

            if (!ImgixEnabled())
            {
                return url;
            }


            if (removeParams)
            {
                url = url.RemoveParams();
            }

            //required by azure blob
            url = url.Replace("/ETG/media", "/etg/media");

            try
            {
                var imgixUrl = ImgixDomain();
                var orignalUri = GetUri(url);

                return CombineUrl(imgixUrl, !orignalUri.IsAbsoluteUri ? url.TrimStart('~') : orignalUri.PathAndQuery);
            }
#if DEBUG
            catch (Exception ex)
# else
            catch 
#endif
            {
                return url;
            }
        }

        public static string Imgixify(this string url, int width, int height, bool doubleSize = false, bool cropFit = true)
        {
            if (string.IsNullOrEmpty(url))
            {
                return string.Empty;
            }

           
            if (url.StartsWith("~"))
            {
                url = url.TrimStart('~');
            }

            if (!ImgixEnabled())
            {
                return url;
            }
            //required by azure blob
            url = url.Replace("/ETG/media", "/etg/media");
            try
            {
                var imgixUrl = ImgixDomain();
                var originalUri = GetUri(url);

                imgixUrl = CombineUrl(imgixUrl, !originalUri.IsAbsoluteUri ? url.TrimStart('~') : originalUri.PathAndQuery);
                imgixUrl = imgixUrl.RemoveParams();
                imgixUrl = imgixUrl.AddAutoFormat();
                imgixUrl = imgixUrl.AddResizerWidth(width);

                if (height > 0)
                {
                    imgixUrl = imgixUrl.AddResizerHeight(height);
                }

                if (cropFit)
                {
                    imgixUrl = imgixUrl.CropFit();
                }

                if (doubleSize)
                {
                    imgixUrl = imgixUrl.DoubleSize();
                }

                return imgixUrl;
            }
#if DEBUG
            catch (Exception ex)
# else
            catch 
#endif
            {
                return url;
            }
        }

        public static string ImgixifyLowRes(this string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return string.Empty;
            }


            if (url.StartsWith("~"))
            {
                url = url.TrimStart('~');
            }

            if (!ImgixEnabled())
            {
                return url;
            }
            //required by azure blob
            url = url.Replace("/ETG/media", "/etg/media");
            try
            {
                var imgixUrl = ImgixDomain();
                var originalUri = GetUri(url);

                imgixUrl = CombineUrl(imgixUrl, !originalUri.IsAbsoluteUri ? url.TrimStart('~') : originalUri.PathAndQuery);
                imgixUrl = imgixUrl.RemoveParams();
                imgixUrl = imgixUrl.AddLowResWidth();
                imgixUrl = imgixUrl.AddAutoFormat();

                return imgixUrl;
            }
#if DEBUG
            catch (Exception ex)
# else
            catch 
#endif
            {
                return url;
            }
        }

        public static string CropFit(this string url)
        {
            return string.IsNullOrEmpty(url)
                ? string.Empty
                : URLHelper.AddParameterToUrl(url, "fit", "crop");
        }

        public static string DoubleSize(this string url)
        {
            return string.IsNullOrEmpty(url)
                ? string.Empty
                : URLHelper.AddParameterToUrl(url, "dpr", "2");
        }

        private static string GetResponsiveImageSrc(string url, int width, int height, bool addComma = true)
        {
            if (addComma)
            {
                return $"{url.Imgixify(width, height)} {width}w,";
            }

            return $"{url.Imgixify(width, height)} {width}w";
        }

        public static string ResponsiveDataSource(this string url, int height = 0)
        {
            
            var str  = new StringBuilder();
            str.AppendLine(GetResponsiveImageSrc(url, 375, height));
            str.AppendLine(GetResponsiveImageSrc(url, 600, height));
            str.AppendLine(GetResponsiveImageSrc(url, 768, height));
            str.AppendLine(GetResponsiveImageSrc(url, 1024, height));
            str.Append(GetResponsiveImageSrc(url, 1920, height, false));
            return str.ToString();
        }
    }
}