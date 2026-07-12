using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using IRIT.Framework.Utility.Controller;
using IRIT.Framework.DataAccess.Context;
using IRIT.EdMS.Security.Contracts;
using System.Threading.Tasks;
using IRIT.EdMS.Security.Business;
using System.Web.UI;
using IRIT.Framework.Utility.Filters;
using System.ComponentModel;
using System.Net;
using IRIT.EdMS.Common.Business;
using IRIT.Framework.DataModel;
using IRIT.Framework.Resources;
using System.Security.AccessControl;
using IRIT.EdMS.Common.Model.ViewModel.SchoolsGroup;
using IRIT.EdMS.Common.Business.Contract;
using IRIT.EdMS.Common.Business.Business;
using IRIT.Framework.DataModel.Common;

namespace IRIT.EdMS.WebUI.Areas.Common.Controllers
{
    [Authorize]
    public partial class SchoolsGroupController : BaseController
    {
        #region Data Member
        private ApplicationDbContext _dbContext { get; }
        private readonly ISchoolsGroup _schoolsGroupBl;
        private readonly ISchool _schoolBl;
        private readonly IInfraStructureRing _infraStructureRingBl;
        #endregion

        #region Constractor
        public SchoolsGroupController()
        {
            _dbContext = new ApplicationDbContext();
            _schoolsGroupBl = new SchoolsGroupBl(_dbContext);
            _schoolBl = new SchoolBl(_dbContext);
            _infraStructureRingBl = new InfraStructureRingBl(_dbContext);
        }
        #endregion

        #region Get
        public virtual ActionResult Index()
        {
            //var schools = _schoolBl.GetAll().ToList();

            //var scGroups = _schoolsGroupBl.GetAll().Select(c => new SchoolsGroupViewModel
            //{
            //    ID = c.ID,
            //    Code = c.Code,
            //    Comment = c.Comment,
            //    InfraStructureRingID = c.InfraStructureRingID,
            //    InfraStructureRingName = c.InfraStructureRingName,
            //    SchoolsGroupName = c.SchoolsGroupName,
            //    Schools = schools.Where(sc => sc.SchoolsGroupID == c.ID).ToList()
            //} ).ToList();
            return View();
        }

        public virtual ActionResult SchoolsGroup_Read([DataSourceRequest]DataSourceRequest request)
        {
            //var schoolsGroup = _schoolsGroupBl.GetAll().Select(c => new SchoolsGroupViewModel
            //{
            //    ID = c.ID,
            //    Code = c.Code,
            //    Comment = c.Comment,
            //    SchoolsGroupName = c.SchoolsGroupName,
            //    InfraStructureRingName = c.,
            //    // InfraStructureRingKindTitle = InfraStructureRingKind.AccessKindDic.FirstOrDefault(k => k.Key == c.InfraStructureRingKindID).Value
            //});

            return Json(_schoolsGroupBl.GetAll().ToDataSourceResult(request));
        }

        public virtual ActionResult School_Read([DataSourceRequest]DataSourceRequest request, long schoolGroupId)
        {
            var sc = _schoolBl.Get(c => c.SchoolsGroupId == schoolGroupId).ToList();
            return Json(sc.ToDataSourceResult(request));
        }

        #endregion

        #region Create

        public virtual ActionResult Create()
        {
            FillSelectList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "Create")]
        //[IRITAuthorize(AssignableToRolePermissions.CanCreateUser)]
        public virtual ActionResult Create(SchoolsGroupViewModel viewModel)
        {
            #region Validation
            if (!ModelState.IsValid)
            {
                this.NotyWarning(GeneralResource.EnterSchoolsGroupName);
                FillSelectList();
                return View(viewModel);
            }
            #endregion

            if (InsertSchoolGroup(viewModel))
            {
                this.NotySuccess(GeneralResource.Add_successful);
                return RedirectToAction(MVC.Common.SchoolsGroup.ActionNames.Index, MVC.Common.SchoolsGroup.Name);
            }

            this.NotyError(GeneralResource.ErrorInRequest,true);
            FillSelectList();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "Create_new")]
        //[IRITAuthorize(AssignableToRolePermissions.CanCreateUser)]
        public virtual ActionResult Create_new(SchoolsGroupViewModel viewModel)
        {
            #region Validation
            if (!ModelState.IsValid)
            {
                this.NotyWarning(GeneralResource.EnterSchoolsGroupName);
                FillSelectList();
                return View("Create", viewModel);
            }
            #endregion

            

            if (InsertSchoolGroup(viewModel))
            {
                this.NotySuccess(GeneralResource.Add_successful);
                return RedirectToAction(MVC.Common.SchoolsGroup.ActionNames.Create, MVC.Common.SchoolsGroup.Name);
            }
            this.NotyError(GeneralResource.ErrorInRequest, true);
            FillSelectList();
            return View("Create", viewModel);

        }

        private bool InsertSchoolGroup(SchoolsGroupViewModel viewModel)
        {
            try
            {
                _schoolsGroupBl.Add(new SchoolsGroup
                {
                    Code = viewModel.Code,
                    InfraStructureRingId = viewModel.InfraStructureRingId,
                    SchoolsGroupName = viewModel.SchoolsGroupName,
                    Comment = viewModel.Comment,
                    CreatorId = User.Id,
                    CreateTime = DateTime.Now,
                    IsDeleted = false
                });
                _dbContext.SaveAllChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region Edit
        [Route("Edit/{id}")]
        [HttpGet]
        [DisplayName("ویرایش گروه مدارس")]
        //[IRITAuthorize(AssignableToRolePermissions.CanEditUser)]
        [ActivityLog(Name = "EditInfra", Description = "ویرایش گروه مدارس")]
        public virtual ActionResult Edit(long? id)
        {
            FillSelectList();

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var schoolsGroup = _schoolsGroupBl.GetById((long)id);
            if (schoolsGroup == null) return HttpNotFound();

            var viewModel = new SchoolsGroupViewModel
            {
                Id = schoolsGroup.Id,
                Code = schoolsGroup.Code,
                SchoolsGroupName = schoolsGroup.SchoolsGroupName,
                InfraStructureRingId = schoolsGroup.InfraStructureRingId,
                Comment = schoolsGroup.Comment
            };
            return View(viewModel);
        }

        [HttpPost]
        [Route("Edit/{id}")]
        [ValidateAntiForgeryToken]
        //[IRITAuthorize(AssignableToRolePermissions.CanEditUser)]
        public virtual ActionResult Edit(SchoolsGroupViewModel viewModel)
        {
            #region Validation
            if (!ModelState.IsValid)
            {
                FillSelectList();
                this.NotyWarning(GeneralResource.EnterInfraStructureRing);
                return View(viewModel);
            }
            #endregion

            var schoolsGroup = _schoolsGroupBl.GetById(viewModel.Id);
            schoolsGroup.Code = viewModel.Code;
            schoolsGroup.SchoolsGroupName = viewModel.SchoolsGroupName;
            schoolsGroup.Comment = viewModel.Comment;
            schoolsGroup.InfraStructureRingId = viewModel.InfraStructureRingId;
            _schoolsGroupBl.Edit(schoolsGroup);
            _dbContext.SaveAllChanges();

            this.NotySuccess(GeneralResource.Edit_successful);
            return RedirectToAction(MVC.Common.SchoolsGroup.ActionNames.Index, MVC.Common.SchoolsGroup.Name);
        }
        #endregion

        #region Delete
        public virtual JsonResult Delete(long id)
        {
            _schoolsGroupBl.Delete(id);
            _dbContext.SaveAllChanges();
            return Json(new { Message = "" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region RemoteValidations

        #endregion

        #region Private
        private void FillSelectList()
        {
            #region InfraStructureRing
            ViewBag.InfraStructureRings = new SelectList(_infraStructureRingBl.GetAll(), "ID", "InfraStructureRingName");
            #endregion
        }
        #endregion
    }
}
