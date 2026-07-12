#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using IRIT.EdMS.Common.Business;
using IRIT.EdMS.Common.Business.Business;
using IRIT.EdMS.Common.Business.Contract;
using IRIT.EdMS.HumanResource.Model.ViewModel.Employee;
using IRIT.EdMS.HumanResources.Business.Business;
using IRIT.EdMS.HumanResources.Business.Contracts;
using IRIT.Framework.Common.Enum;
using IRIT.Framework.DataAccess.Context;
using IRIT.Framework.DataModel;
using IRIT.Framework.DataModel.HumanResources;
using IRIT.Framework.Resources;
using IRIT.Framework.Utility.Controller;
using IRIT.Framework.Utility.Filters;
using IRIT.Framework.Utility.Utility;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
#endregion

namespace IRIT.EdMS.WebUI.Areas.HumanResource.Controllers
{
    [Authorize]
    public partial class EmployeeController : BaseController
    {
        #region Data Member
        private ApplicationDbContext DbContext { get; }
        private readonly IEmployee _employeeBl;
        private readonly IParty _partyBl;
        #endregion

        #region Constractor
        public EmployeeController()
        {
            DbContext = new ApplicationDbContext();
            _employeeBl = new EmployeeBl(DbContext);
            _partyBl = new PartyBlo(DbContext);
        }
        #endregion

        #region Get
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult Employee_Read([DataSourceRequest]DataSourceRequest request)
        {
            var employees = _employeeBl.GetAll();
            return Json(employees.ToDataSourceResult(request));
        }
        #endregion

        #region Create

        public virtual ActionResult Create()
        {
            FillSelectList();
            return View();
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Create")]
        [ValidateAntiForgeryToken]
        //[IRITAuthorize(AssignableToRolePermissions.CanCreateUser)]
        public virtual ActionResult Create(EmployeeViewModel viewModel)
        {
            #region Validation

            #endregion

            if (InsertEmployee(viewModel))
            {
                this.NotySuccess(GeneralResource.Add_successful);
                return RedirectToAction(MVC.HumanResource.Employee.ActionNames.Index, MVC.HumanResource.Employee.Name);
            }
            this.NotyError(GeneralResource.ErrorInRequest, true);
            FillSelectList();
            return View(viewModel);

        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Create_new")]
        [ValidateAntiForgeryToken]
        //[IRITAuthorize(AssignableToRolePermissions.CanCreateUser)]
        public virtual ActionResult Create_new(EmployeeViewModel viewModel)
        {
            #region Validation

            #endregion


            if (InsertEmployee(viewModel))
            {
                this.NotySuccess(GeneralResource.Add_successful);
                return RedirectToAction(MVC.HumanResource.Employee.ActionNames.Create, MVC.HumanResource.Employee.Name);
            }
            this.NotyError(GeneralResource.ErrorInRequest, true);
            FillSelectList();
            return View("Create",viewModel);

        }

        private bool InsertEmployee(EmployeeViewModel viewModel)
        {
            try
            {
            var emp = new Employee
            {
                PartyId = viewModel.PartyId.Value,
                CreatorId = User.Id,
                CreateTime = DateTime.Now,
                Comment = viewModel.Comment,
                IsDeleted = false,
                MarriageStatusType = (MarriageStatusTypesEnum)viewModel.MarriageStatusKind.Value,
                MilitaryServiceType = (MilitaryServiceTypesEnum)viewModel.MilitaryServiceKind.Value,
                EmploymentNumber = viewModel.EmploymentNumber,
                MilitaryServiceStartDate = viewModel.MilitaryServiceStartDate,
                MilitaryServiceFinishDate = viewModel.MilitaryServiceFinishDate,
                EmployementType = (EmploymentTypesEnum)viewModel.EmployementKindId.Value
            };
            _employeeBl.Add(emp);
            DbContext.SaveChanges();
            return true;
        }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region Edit
        //[Route("Edit/{id}")]
        [HttpGet]
        [DisplayName("ویرایش گروه کاربران")]
        //[IRITAuthorize(AssignableToRolePermissions.CanEditUser)]
        public virtual ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            FillSelectList();
            var emp = _employeeBl.GetById(id.Value);
            var empModel = new EmployeeViewModel
            {
                Id = emp.Id,
                PartyId = emp.PartyId,
                MilitaryServiceFinishDate = emp.MilitaryServiceFinishDate.Value,
                MilitaryServiceStartDate = emp.MilitaryServiceStartDate.Value,
                EmploymentNumber = emp.EmploymentNumber,
                Comment = emp.Comment,
                MarriageStatusKind = (byte?)emp.MarriageStatusType,
                MilitaryServiceKind = (byte?)emp.MilitaryServiceType
            };
            return View(empModel);
        }

        [HttpPost]
        [Route("Edit/{id}")]
        [ValidateAntiForgeryToken]
        //[IRITAuthorize(AssignableToRolePermissions.CanEditUser)]
        public virtual ActionResult Edit(EmployeeViewModel viewModel)
        {
            var emp = _employeeBl.GetById(viewModel.Id);
            emp.PartyId = viewModel.PartyId.Value;
            emp.Comment = viewModel.Comment;
            emp.MarriageStatusType = (MarriageStatusTypesEnum) viewModel.MarriageStatusKind.Value;
            emp.MilitaryServiceType = (MilitaryServiceTypesEnum) viewModel.MilitaryServiceKind.Value;
            emp.EmploymentNumber = viewModel.EmploymentNumber;
            emp.MilitaryServiceStartDate = viewModel.MilitaryServiceStartDate;
            emp.MilitaryServiceFinishDate = viewModel.MilitaryServiceFinishDate;

            _employeeBl.Add(emp);
            DbContext.SaveChanges();

            this.NotySuccess(GeneralResource.Edit_successful);
            return RedirectToAction(MVC.HumanResource.Employee.ActionNames.Index, MVC.HumanResource.Employee.Name);
        }
        #endregion

        #region Delete
        public virtual JsonResult Delete(long id)
        {
            var emp = _employeeBl.GetById(id);
            emp.IsDeleted = true;
            _employeeBl.Edit(emp);
            DbContext.SaveAllChanges();
            return Json(new { Message = "" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region RemoteValidations

        #endregion

        #region Private
        private void FillSelectList()
        {
            //ViewBag.Partys = new SelectList(_partyBl.GetViewPartyEmployee(c => c.SchoolID == User.SchoolId)
            //            .Select(c => new { Id = c.ID, FullName = $"{c.FirstName} {c.LastName}" }), "Id", "FullName");

            ViewBag.MilitaryServiceKinds = new SelectList(MilitaryServiceTypes.MilitaryServiceTypesDic, "Key", "Value");

            ViewBag.MarriageStatusKinds = new SelectList(MarriageStatusTypes.MarriageStatusTypesDic, "Key", "Value");

            ViewBag.EmployementKinds = new SelectList(EmploymentTypes.ActionKindDic, "Key", "Value");
        }
        #endregion
    }
}