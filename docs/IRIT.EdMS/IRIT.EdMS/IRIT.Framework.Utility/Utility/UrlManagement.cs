using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IRIT.Framework.Utility.Utility
{
    public static class UrlManagement
    {
        public static string ToAbsoluteUrl(this string relativeUrl)
        {
            if (string.IsNullOrEmpty(relativeUrl))
                return relativeUrl;

            if (HttpContext.Current == null)
                return relativeUrl;

            if (relativeUrl.StartsWith("/"))
                relativeUrl = relativeUrl.Insert(0, "~");
            if (!relativeUrl.StartsWith("~/"))
                relativeUrl = relativeUrl.Insert(0, "~/");

            var url = HttpContext.Current.Request.Url;
            var port = url.Port != 80 ? (":" + url.Port) : string.Empty;

            return $"{url.Scheme}://{url.Host}{port}{VirtualPathUtility.ToAbsolute(relativeUrl)}";
        }
    }
}
