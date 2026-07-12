using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IRIT.EdMS.Common.Business;
using IRIT.EdMS.HumanResource.Model.ViewModel.Employee;
using IRIT.EdMS.HumanResources.Business.Business;
using IRIT.EdMS.HumanResources.Business.Contracts;
using IRIT.Framework.DataAccess.Context;
using IRIT.Framework.Utility.Controller;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace IRIT.EdMS.WebUI.Areas.Common.Controllers
{
    public partial class PersonController : BaseController
    {
        #region Data Member
        private ApplicationDbContext DbContext { get; }
        private readonly IEmployee _employeeBl;
        #endregion

        #region Constractor
        public PersonController()
        {
            DbContext = new ApplicationDbContext();
            _employeeBl = new EmployeeBl(DbContext);
        }
        #endregion
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult Employee_Read([DataSourceRequest]DataSourceRequest request)
        {
            var employees = _employeeBl.GetAll().Select(c=> new EmployeeViewModel
            {
                FirstName = c.Party.FirstName,
                LastName = c.Party.LastName,
                Id = c.Id
            });
            return Json(employees.ToDataSourceResult(request));
        }

        public virtual JsonResult Delete(string id)
        {
            return Json(new { success = true, Message = "", data = "محمد باقر صرافچگان" }, JsonRequestBehavior.AllowGet);
        }

    }
}