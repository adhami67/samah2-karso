using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IRIT.EdMS.Common.Model.ViewModel.Theme;

namespace IRIT.EdMS.WebUI.Controllers
{
    [AllowAnonymous]
    public partial class ThemeController : Controller
    {
        [HttpGet]
        public virtual ActionResult Index()
        {
            HttpCookie themeCookie = HttpContext.Request.Cookies["ThemeCookie"] ?? new HttpCookie("ThemeCookie");

            var model = new ThemeViewModel
            {
                StartEffect = !(themeCookie.Values["StartEffect"] != null && themeCookie.Values["StartEffect"] == "0"),
                Theme = themeCookie.Values["Theme"] ?? "Metro"
            };
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Index(ThemeViewModel model)
        {
            var themeCookie = new HttpCookie("ThemeCookie");
            themeCookie.Values.Add("StartEffect", (model.StartEffect ? 1 : 0).ToString());
            themeCookie.Values.Add("Theme", model.Theme);
            HttpContext.Response.SetCookie(themeCookie);
            return RedirectToAction("Index");
        }
    }
}