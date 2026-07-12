using System;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace IRIT.EdMS.WebUI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }

        public void ConfigureAuth(IAppBuilder appBuilder)
        {
            const int twoWeeks = 14;

            UrlHelper url = new UrlHelper(HttpContext.Current.Request.RequestContext);

            CookieAuthenticationProvider provider = new CookieAuthenticationProvider();

            var originalHandler = provider.OnApplyRedirect;

            //Our logic to dynamically modify the path (maybe needs some fine tuning)
            provider.OnApplyRedirect = context =>
            {
                var mvcContext = new HttpContextWrapper(HttpContext.Current);
                var routeData = RouteTable.Routes.GetRouteData(mvcContext);

                //Get the current language  
                RouteValueDictionary routeValues = new RouteValueDictionary(new
                {
                    action = MVC.Security.Account.ActionNames.Login,
                    controller = MVC.Security.Account.Name,
                    area = MVC.Security.Account.Area
                });
                routeValues.Add("lang", routeData.Values["lang"]);

                //Reuse the RetrunUrl
                Uri uri = new Uri(context.RedirectUri);
                string returnUrl = HttpUtility.ParseQueryString(uri.Query)[context.Options.ReturnUrlParameter];
                routeValues.Add(context.Options.ReturnUrlParameter, returnUrl);

                //Overwrite the redirection uri
                context.RedirectUri = url.Action(MVC.Security.Account.ActionNames.Login, MVC.Security.Account.Name, routeValues);
                originalHandler.Invoke(context);
            };

            appBuilder.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString(url.Action(MVC.Security.Account.ActionNames.Login, MVC.Security.Account.Name)),
                ExpireTimeSpan = TimeSpan.FromMinutes(twoWeeks),
                SlidingExpiration = true,
                CookieName = "DotNetCms2",
                Provider = provider
            });
        }
    }
}