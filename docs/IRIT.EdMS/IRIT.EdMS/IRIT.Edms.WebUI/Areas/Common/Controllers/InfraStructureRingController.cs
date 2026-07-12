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
using IRIT.EdMS.Common.Business.Business;
using IRIT.EdMS.Common.Business.Contract;
using IRIT.EdMS.Common.Model.ViewModel.InfraStructureRings;
using IRIT.Framework.Common.Enum;
using IRIT.Framework.DataModel.Common;
using IRIT.Framework.Resources;

namespace IRIT.EdMS.WebUI.Areas.Common.Controllers
{
    [Authorize]
    public partial class InfraStructureRingController : BaseController
    {
        #region Data Member
        private ApplicationDbContext _dbContext { get; }
        private readonly IInfraStructureRing _infraStructureRingBl;
        #endregion

        #region Constractor
        public InfraStructureRingController()
        {
            _dbContext = new ApplicationDbContext();
            _infraStructureRingBl = new InfraStructureRingBl(_dbContext);
        }
        #endregion

        #region Get
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult InfraStructureRing_Read([DataSourceRequest]DataSourceRequest request)
        {
            var infraStructureRings = _infraStructureRingBl.GetAll().Select(c => new InfraStructureRingViewModel
            {
                Id = c.Id,
                Code = c.Code,
                Comment = c.Comment,
                InfraStructureRingKindId = c.InfraStructureRingType,
                InfraStructureRingName = c.InfraStructureRingName,
                InfraStructureRingKindTitle = InfraStructureRingTypes.InfraStructureRingTypesDic.FirstOrDefault(k => k.Key == c.InfraStructureRingType).Value
            });
            
            return Json(infraStructureRings.ToDataSourceResult(request));
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
        public virtual ActionResult Create(InfraStructureRingViewModel viewModel)
        {
            #region Validation
            if (!ModelState.IsValid)
            {
                this.NotyWarning(GeneralResource.EnterInfraStructureRing);
                FillSelectList();
                return View(viewModel);
            }
            #endregion

            if (InsertInfraStructureRing(viewModel))
            {
                 this.NotySuccess(GeneralResource.Add_successful);
            return RedirectToAction(MVC.Common.InfraStructureRing.ActionNames.Index, MVC.Common.InfraStructureRing.Name);
            }
            this.NotyError(GeneralResource.ErrorInRequest, true);
            FillSelectList();
            return View(viewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "Create_new")]
        public virtual ActionResult Create_new(InfraStructureRingViewModel viewModel)
        {
            #region Validation
            if (!ModelState.IsValid)
            {
                this.NotyWarning(GeneralResource.EnterInfraStructureRing);
                FillSelectList();
                return View("Create",viewModel);
            }
            #endregion

            if (InsertInfraStructureRing(viewModel))
            {
                this.NotySuccess(GeneralResource.Add_successful);
                return RedirectToAction(MVC.Common.InfraStructureRing.ActionNames.Create, MVC.Common.InfraStructureRing.Name);
            }
            this.NotyError(GeneralResource.ErrorInRequest, true);
            FillSelectList();
            return View("Create", viewModel);

        }
        private bool InsertInfraStructureRing(InfraStructureRingViewModel viewModel)
        {

            try
            {
                _infraStructureRingBl.Add(new InfraStructureRing
                {
                    Code = viewModel.Code,
                    InfraStructureRingName = viewModel.InfraStructureRingName,
                    InfraStructureRingType = viewModel.InfraStructureRingKindId,
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
        [DisplayName("ویرایش میدان آموزشی")]
        //[IRITAuthorize(AssignableToRolePermissions.CanEditUser)]
        [ActivityLog(Name = "EditInfra", Description = "ویرایش میدان آموزشی")]
        public virtual ActionResult Edit(long? id)
        {
            FillSelectList();
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var infraStructureRing = _infraStructureRingBl.GetById((long)id);
            if (infraStructureRing == null) return HttpNotFound();

            var viewModel = new InfraStructureRingViewModel {
                Id = infraStructureRing.Id,
                Code = infraStructureRing.Code,
                InfraStructureRingName = infraStructureRing.InfraStructureRingName,
                InfraStructureRingKindId = infraStructureRing.InfraStructureRingType,
                Comment = infraStructureRing.Comment };
            return View(viewModel);
        }

        [HttpPost]
        [Route("Edit/{id}")]
        [ValidateAntiForgeryToken]
        //[IRITAuthorize(AssignableToRolePermissions.CanEditUser)]
        public virtual ActionResult Edit(InfraStructureRingViewModel viewModel)
        {
            #region Validation
            if (!ModelState.IsValid)
            {
                this.NotyWarning(GeneralResource.EnterInfraStructureRing);
                FillSelectList();
                return View(viewModel);
            }
            #endregion

            var infraStructureRing = _infraStructureRingBl.GetById(viewModel.Id);
            infraStructureRing.Code= viewModel.Code;
            infraStructureRing.InfraStructureRingName = viewModel.InfraStructureRingName;
            infraStructureRing.Comment = viewModel.Comment;
            infraStructureRing.InfraStructureRingType = viewModel.InfraStructureRingKindId;
            _infraStructureRingBl.Edit(infraStructureRing );
            _dbContext.SaveAllChanges();

            this.NotySuccess("عملیات  ویرایش میدان آموزشی با موفقیت انجام شد");
            return RedirectToAction(MVC.Common.InfraStructureRing.ActionNames.Index, MVC.Common.InfraStructureRing.Name);
        }
        #endregion

        #region Delete
        public virtual JsonResult Delete(long id)
        {
            _infraStructureRingBl.Delete(id);
            _dbContext.SaveAllChanges();
            return Json(new { Message = "" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region RemoteValidations

        #endregion

        #region Private
        private void FillSelectList()
        {
            ViewBag.InfraStructureRingKinds = new SelectList(InfraStructureRingTypes.InfraStructureRingTypesDic, "Key", "Value");
        }
       
        #endregion
    }
}