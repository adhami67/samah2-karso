using System.Web;
using IRIT.Framework.Utility.Helpers.Extentions;

namespace IRIT.Framework.Utility.Helpers.Http
{
    public static class HttpExtentions
    {
        public static string PhysicalToVirtualPathConverter(this HttpServerUtilityBase utility, string path, HttpRequestBase context)
        {
            return path.Replace(context.ServerVariables["APPL_PHYSICAL_PATH"], "/").Replace(@"\", "/");
        }

        public static string GetIp(this HttpRequestBase request)
        {
            var ip = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            //اگر عمل 
            //Forwarding 
            //صورت نگیرد در این صورت آدرس اصلی در متغییر زیر است
            if (!ip.HasValue() && !ip.IsNotEmpty()) ip = request.UserHostAddress;
            return ip;
        }
        public static string GetIp(this HttpRequest request)
        {
            var ip = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            //اگر عمل 
            //Forwarding 
            //صورت نگیرد در این صورت آدرس اصلی در متغییر زیر است
            if (!ip.HasValue() && !ip.IsNotEmpty()) ip = request.UserHostAddress;
            return ip;
        }
    }
}