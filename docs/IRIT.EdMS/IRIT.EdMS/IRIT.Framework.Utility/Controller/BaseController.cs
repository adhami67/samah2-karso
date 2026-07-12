using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using IRIT.Framework.Utility.Controller;
using IRIT.Framework.Utility.Controller.NotyHelper;
using IRIT.Framework.Utility.Helpers;

namespace IRIT.Framework.Utility.Controller
{
    public class BaseController : System.Web.Mvc.Controller
    {
        #region Validation
        [ChildActionOnly]
        protected ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home", new { area = "" });
        }
        #endregion

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            var cultureName = RouteData.Values["culture"] as string ??
                              (Request.UserLanguages != null && Request.UserLanguages.Length > 0 ? Request.UserLanguages[0] : null);

            // Attempt to read the culture cookie from Request

            // Validate culture name
            cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe

            // force redirect login page
            if (RouteData.Values["culture"] as string != cultureName)
            {

                // Force a valid culture in the URL
                RouteData.Values["culture"] = cultureName.ToLowerInvariant(); // lower case too

                // Redirect user
                // Response.RedirectToRoute(RouteData.Values);
            }


            // Modify current thread's cultures            
            if (cultureName != null)
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;


            return base.BeginExecuteCore(callback, state);
        }

        protected virtual new Security.EdmsPrincipal User
        {
            get
            {
                var serializer = new JavaScriptSerializer();
                var data = ((System.Web.Security.FormsIdentity) HttpContext.User.Identity).Ticket.UserData;
                return serializer.Deserialize<Security.EdmsPrincipal>(data);
            }
        }
    }
}

