using System.Web.Mvc;

namespace IRIT.EdMS.WebUI.Areas.HumanResource
{
    public class HumanResourceAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "HumanResource";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "HumanResource_default",
                "HumanResource/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}