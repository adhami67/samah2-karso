using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;
using IRIT.Framework.Utility.Security;

namespace IRIT.EdMS.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        #region Application_Start
        protected void Application_Start()
        {
            try
            {
                AreaRegistration.RegisterAllAreas();
                
                //var areaNames = RouteTable.Routes.OfType<Route>()
                //.Where(d => d.DataTokens != null && d.DataTokens.ContainsKey("area"))
                //.Select(r => r.DataTokens["area"]).ToArray();
                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                BundleConfig.RegisterBundles(BundleTable.Bundles);
                ApplicationStart.Config();

            }
            catch
            {
                HttpRuntime.UnloadAppDomain(); // سبب ری استارت برنامه و آغاز مجدد آن با درخواست بعدی می‌شود
                throw;
            }
        }       
        #endregion

        #region Application_EndRequest
        protected void Application_EndRequest(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception)
            {

            }

        }
        #endregion

        #region Application_BeginRequest
        private void Application_BeginRequest(object sender, EventArgs e)
        {

        }
        #endregion

        #region ShouldIgnoreRequest
        private bool ShouldIgnoreRequest()
        {
            string[] reservedPath =
              {
                  "/__browserLink",
                  "/Scripts",
                  "/Content"
              };

            var rawUrl = Context.Request.RawUrl;
            if (reservedPath.Any(path => rawUrl.StartsWith(path, StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }

            return BundleTable.Bundles.Select(bundle => bundle.Path.TrimStart('~'))
                      .Any(bundlePath => rawUrl.StartsWith(bundlePath, StringComparison.OrdinalIgnoreCase));
        }
        #endregion

        #region Private
        private void SetPermissions(IEnumerable<string> permissions)
        {
            Context.User = new GenericPrincipal(Context.User.Identity, permissions.ToArray());
        }
        #endregion

        #region Application_Error
        protected void Application_Error()
        {

        }
        #endregion

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie == null) return;

            var authTicket = FormsAuthentication.Decrypt(authCookie.Value);

            var serializer = new JavaScriptSerializer();

            if (authTicket != null) return;

            var serializeModel = serializer.Deserialize<EdMSPrincipalSerializeModel>(authTicket.UserData);

            var newUser = new EdmsPrincipal()
            {
                Id = serializeModel.Id,
                FullName = serializeModel.FullName,
                UserName = serializeModel.UserName,
                SchoolId = serializeModel.SchoolId ?? 0
            };

            HttpContext.Current.User = newUser;
        }
    }
}
