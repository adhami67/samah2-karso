using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IRIT.Framework.Utility.Helpers
{
    public class ThemeHelper
    {
        public readonly static string DefaultTheme = "Metro";
        public readonly static string Black = "Black";
        public readonly static string BlueOpal = "BlueOpal";
        public readonly static string Bootstrap = "Bootstrap";
        public readonly static string Default = "Default";
        public readonly static string Flat = "Flat";
        public readonly static string HighContrast = "HighContrast";
        public readonly static string Metro = "Metro";
        public readonly static string MetroBlack = "MetroBlack";
        public static readonly string Moonlight = "Moonlight";
        public readonly static string Silver = "Silver";
        public readonly static string Uniform = "Uniform";

        public static string GetTheme()
        {
            HttpCookie themeCookie = HttpContext.Current.Request.Cookies["ThemeCookie"] ?? new HttpCookie("ThemeCookie");
            return themeCookie.Values["Theme"] ?? DefaultTheme;
        }
    }
}
