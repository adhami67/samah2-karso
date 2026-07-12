using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IRIT.Framework.Utility.MVC
{
    public static class ControllerExtentions
    {
        //[NonAction]
        //public static void AddErrors(this Controller controller, IdentityResult result)
        //{
        //    foreach (var error in result.Errors)
        //    {
        //        controller.ModelState.AddModelError("", error);
        //    }
        //}

        [NonAction]
        public static void AddErrors(this System.Web.Mvc.Controller controller, string property, string error)
        {
            controller.ModelState.AddModelError(property, error);
        }
    }
}
