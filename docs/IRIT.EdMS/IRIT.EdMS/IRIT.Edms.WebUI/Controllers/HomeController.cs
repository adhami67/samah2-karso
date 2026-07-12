using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IRIT.EdMS.Common.Model.ViewModel.SubSystem;
using IRIT.Framework.Utility.Controller;

namespace IRIT.EdMS.WebUI.Controllers
{
    [Authorize]
    public partial class HomeController : Controller
    {
        public virtual ActionResult Index()
        {
            return View();
        }
        
        public virtual ActionResult SubSystems()
        {
            var subSystems = new List<SubSystemListViewModel>()
            {
                new SubSystemListViewModel { ActionName = "Index", ControlerName = "User", Area = "Security", SystemTitle = "عمومی", RoleTitle = "حسابدار"},
                new SubSystemListViewModel { ActionName = "Login", ControlerName = "Account", Area = "Security", SystemTitle = "آموزشی", RoleTitle = "معلم"},
                new SubSystemListViewModel { ActionName = "", ControlerName = "", Area = "", SystemTitle = "انبار", RoleTitle = "انباردار"},
                new SubSystemListViewModel { ActionName = "", ControlerName = "", Area = "", SystemTitle = "چای خانه", RoleTitle = "بوفه چی"},
                new SubSystemListViewModel { ActionName = "", ControlerName = "", Area = "", SystemTitle = "مدیریت مدرسه", RoleTitle = "مدیر سایت"},
                                new SubSystemListViewModel { ActionName = "", ControlerName = "", Area = "", SystemTitle = "حسابداری", RoleTitle = "حسابدار"},
                new SubSystemListViewModel { ActionName = "", ControlerName = "", Area = "", SystemTitle = "آموزشی", RoleTitle = "معلم"},
                new SubSystemListViewModel { ActionName = "", ControlerName = "", Area = "", SystemTitle = "انبار", RoleTitle = "انباردار"},
                new SubSystemListViewModel { ActionName = "", ControlerName = "", Area = "", SystemTitle = "چای خانه", RoleTitle = "بوفه چی"},
                new SubSystemListViewModel { ActionName = "", ControlerName = "", Area = "", SystemTitle = "مدیریت مدرسه", RoleTitle = "مدیر سایت"},
            };

            return View(subSystems);
        }
    }
}