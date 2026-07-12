using System.Web.Mvc;
using IRIT.Framework.Utility.Controller;

namespace IRIT.EdMS.WebUI.Areas.Calendar.Controllers
{
    [Authorize]
    public partial class CalendarController : BaseController
    {
        // GET: Calendar/Calendar
        public virtual ActionResult Index()
        {
            return View();
        }
    }
}