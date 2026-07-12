#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IRIT.Framework.DataAccess.Context;
using IRIT.Framework.Utility.Controller;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using IRIT.EdMS.Common.Business;
using System.ComponentModel;
using System.Net;
using IRIT.Framework.DataModel;
using System.Web.UI;
using IRIT.EdMS.Common.Business.Business;
using IRIT.EdMS.Common.Business.Contract;
using IRIT.EdMS.Common.Model.ViewModel.School;
using IRIT.Framework.DataModel.Common;
using IRIT.Framework.Resources;
using IRIT.Framework.Utility.Filters;
using IRIT.Framework.Utility.Utility;
using IRIT.EdMS.Common.Business.Contract.Lookup;
using IRIT.EdMS.Common.Business.Business.Lookup;
using IRIT.Framework.Common.Enum;
#endregion

namespace IRIT.EdMS.WebUI.Areas.Common.Controllers
{
    [Authorize]
    public partial class SchoolController : BaseController
    {
        #region Data Member
        private ApplicationDbContext _dbContext { get; }
        private readonly ISchool _schoolBl;
        private readonly ISchoolsGroup _schoolsGroupBl;
        private readonly IGradeKind _gradeKindBl;
        #endregion

        #region Constractor
        public SchoolController()
        {
            _dbContext = new ApplicationDbContext();
            _schoolBl = new SchoolBl(_dbContext);
            _schoolsGroupBl = new SchoolsGroupBl(_dbContext);
            _gradeKindBl = new GradeKindBl(_dbContext);
        }
        #endregion

        #region Get
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult School_Read([DataSourceRequest]DataSourceRequest request)
        {
            var school = _schoolBl.GetAll();
            return Json(school.ToDataSourceResult(request));
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
        public virtual ActionResult Create(SchoolViewModel viewModel)
        {
            #region Validation
            if (!ModelState.IsValid)
            {
                if (viewModel.SchoolName == null)
                    this.NotyWarning(GeneralResource.EnterSchoolsName);

                FillSelectList();
                return View(viewModel);
            }
            #endregion

            var schoolgroup = _schoolsGroupBl.GetById(viewModel.SchoolsGroupId);

            if (InsertSchool(viewModel, schoolgroup))
            {
                this.NotySuccess(GeneralResource.Add_successful);
                return RedirectToAction(MVC.Common.School.ActionNames.Index, MVC.Common.School.Name);
            }

            this.NotyError(GeneralResource.ErrorInRequest, true);
            FillSelectList();
            return View(viewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "Create_new")]
        public virtual ActionResult Create_new(SchoolViewModel viewModel)
        {
            #region Validation
            if (!ModelState.IsValid)
            {
                if (viewModel.SchoolName == null)
                    this.NotyWarning(GeneralResource.EnterSchoolsName);
                FillSelectList();
                return View("Create", viewModel);
            }
            #endregion

            var schoolgroup = _schoolsGroupBl.GetById(viewModel.SchoolsGroupId);

            if (InsertSchool(viewModel, schoolgroup))
            {
                this.NotySuccess(GeneralResource.Add_successful);
                return RedirectToAction(MVC.Common.School.ActionNames.Create, MVC.Common.School.Name);
            }

            this.NotyError(GeneralResource.ErrorInRequest, true);
            FillSelectList();
            return View("Create", viewModel);
        }

        private bool InsertSchool(SchoolViewModel viewModel, SchoolsGroup schoolgroup)
        {
            try
            {
                var school = new School
                {
                    Code = viewModel.Code,
                    SchoolName = viewModel.SchoolName,
                    SchoolsGroupId = viewModel.SchoolsGroupId,
                    InfraStructureRingId = schoolgroup.InfraStructureRingId,
                    Email = viewModel.Email,
                    StudyGradeId = viewModel.GradeKindId,
                    IssueYear = viewModel.IssueYear?.Date.Year,
                    PostAddress = viewModel.PostAddress,
                    PhoneNo = viewModel.PhoneNo,
                    MobileNo = viewModel.MobileNo,
                    FaxNo = viewModel.FaxNo,
                    HomePageUrl = viewModel.HomePageUrl,
                    Comment = viewModel.Comment,
                    GenderType = (GenderTypesEnum) viewModel.GenderKindId,
                    CreateTime = DateTime.Now,
                    CreatorId = User.Id,
                    IsDeleted = false
                };

                if (viewModel.Image != null)
                {
                    school.SmallImage = viewModel.Image.ConvertToByteArray();
                }
                _schoolBl.Add(school);
                _dbContext.SaveAllChanges();
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        #endregion

        #region Edit
        //[Route("Edit/{id}")]
        [HttpGet]
        [DisplayName("ویرایش مدرسه")]
        //[IRITAuthorize(AssignableToRolePermissions.CanEditUser)]
        public virtual ActionResult Edit(long? id)
        {
            FillSelectList();
            if (id == null)

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var school = _schoolBl.GetById((long)id);
            if (school == null) return HttpNotFound();

            var viewModel = new SchoolViewModel
            {
                Id = school.Id,
                Code = school.Code,
                SchoolName = school.SchoolName,
                SchoolsGroupId = (long)school.SchoolsGroupId,
                //ImageID = blob.ID != 0 ? blob.ID : (long?)null,

                Email = school.Email,
                //Todo SmallImage = 
                //IssueYear = school.IssueYear?.Date.Year,
                PostAddress = school.PostAddress,
                PhoneNo = school.PhoneNo,
                MobileNo = school.MobileNo,
                FaxNo = school.FaxNo,
                HomePageUrl = school.HomePageUrl,
                Comment = school.Comment,
                GenderKindId = school.GenderType
            };

            if (school.SmallImage != null)
            {
                viewModel.ImageByte = school.SmallImage;
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("Edit/{id}")]
        [ValidateAntiForgeryToken]
        //[IRITAuthorize(AssignableToRolePermissions.CanEditUser)]
        public virtual ActionResult Edit(SchoolViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                FillSelectList();
                //this.NotyWarning(GeneralResource.EnterInfraStructureRing);
                return View(viewModel);
            }
            var school = _schoolBl.GetById(viewModel.Id);
            school.Code = viewModel.Code;
            school.SchoolName = viewModel.SchoolName;
            // party.BirthDate = null;
            school.Email = viewModel.Email;
            // Todo SmallImage =                 
            school.SchoolsGroupId = viewModel.SchoolsGroupId;
            school.PhoneNo = viewModel.PhoneNo;
            school.MobileNo = viewModel.MobileNo;
            //school.IssueDate = viewModel.IssueDate;
            school.PostAddress = viewModel.PostAddress;
            school.FaxNo = viewModel.FaxNo;
            school.HomePageUrl = viewModel.HomePageUrl;
            school.GenderType = viewModel.GenderKindId;
            school.Comment = viewModel.Comment;

            if (viewModel.Image != null)
            {
                school.SmallImage = viewModel.Image.ConvertToByteArray();
            }

            _schoolBl.Edit(school);
            _dbContext.SaveAllChanges();

            this.NotySuccess(GeneralResource.Edit_successful);
            return RedirectToAction(MVC.Common.School.ActionNames.Index, MVC.Common.School.Name);
        }
        #endregion

        #region Delete
        public virtual JsonResult Delete(long id)
        {
            var school = _schoolBl.GetById(id);
            school.IsDeleted = true;

            _schoolBl.Edit(school);
            _dbContext.SaveAllChanges();
            return Json(new { Message = "" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region private
        private void FillSelectList()
        {
            #region Gender
            ViewBag.Genders = new SelectList(SchoolGenderTypes.SchoolGenderTypeDic, "Key", "Value");
            #endregion

            #region GradeKind
            ViewBag.GradeKinds = new SelectList(_gradeKindBl.GetAll(), "ID", "Title");
            #endregion

            #region InfraStructureRing
            ViewBag.SchoolGroups = new SelectList(_schoolsGroupBl.GetAll(), "ID", "SchoolsGroupName");
            #endregion
        }
        #endregion
    }
}