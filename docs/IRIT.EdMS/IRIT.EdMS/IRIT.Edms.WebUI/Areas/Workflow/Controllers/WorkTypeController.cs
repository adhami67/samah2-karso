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
using IRIT.EdMS.Workflow.Business;
using System.ComponentModel;
using System.Net;
using IRIT.Framework.DataModel;
using System.Web.UI;
using IRIT.Framework.Resources;
using IRIT.Framework.Utility.Filters;
using IRIT.Framework.Utility.Utility;
using IRIT.EdMS.Workflow.Business.Contracts;
using IRIT.EdMS.Workflow.Business.Business;
using IRIT.EdMS.Workflow.Model.ViewModel.WorkType;
using IRIT.Framework.DataModel.Workflow;
#endregion
namespace IRIT.EdMS.WebUI.Areas.Workflow.Controllers
{
    [Authorize]
    public partial class WorkTypeController : BaseController
    {
        #region Data Member
        private ApplicationDbContext _dbContext { get; }
        private readonly IWorkType _workTypeBl;
        #endregion

        #region Constractor
        public WorkTypeController()
        {
            _dbContext = new ApplicationDbContext();
            _workTypeBl = new WorkTypeBl(_dbContext);

        }
        #endregion

        #region Get
        public virtual ActionResult Index()
        {
            return View();
        }
        public virtual ActionResult WorkType_Read([DataSourceRequest]DataSourceRequest request)
        {
            var workType = _workTypeBl.GetAll();
            return Json(workType.ToDataSourceResult(request));
        }
        #endregion

        #region Create

        public virtual ActionResult Create()
        {
            //FillSelectList();
            return View();
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Create")]
        [ValidateAntiForgeryToken]
        //[IRITAuthorize(AssignableToRolePermissions.CanCreateUser)]
        public virtual ActionResult Create(WorkTypeViewModel viewModel)
        {
            #region Validation
            if (!ModelState.IsValid)
            {
                if (viewModel.WorkTypeTitle == null)
                    this.NotyWarning(GeneralResource.RequiredMessage);

                // FillSelectList();
                return View(viewModel);
            }
            #endregion

            if (InsertWorkType(viewModel))
            {
                this.NotySuccess(GeneralResource.Add_successful);
                return RedirectToAction(MVC.Workflow.WorkType.ActionNames.Index, MVC.Workflow.WorkType.Name);
            }

            this.NotyError(GeneralResource.ErrorInRequest, true);
            //FillSelectList();
            return View(viewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "Create_new")]
        public virtual ActionResult Create_new(WorkTypeViewModel viewModel)
        {
            #region Validation
            if (!ModelState.IsValid)
            {
                if (viewModel.WorkTypeTitle == null)
                    this.NotyWarning(GeneralResource.RequiredMessage);
                //FillSelectList();
                return View("Create", viewModel);
            }
            #endregion

            if (InsertWorkType(viewModel))
            {
                this.NotySuccess(GeneralResource.Add_successful);
                return RedirectToAction(MVC.Workflow.WorkType.ActionNames.Create, MVC.Workflow.WorkType.Name);
            }

            this.NotyError(GeneralResource.ErrorInRequest, true);
            //FillSelectList();
            return View("Create", viewModel);
        }
        private bool InsertWorkType(WorkTypeViewModel viewModel)
        {
            try
            {
                var workType = new WorkType
                {
                    WorkTypeTitle = viewModel.WorkTypeTitle,
                    WorkTypeCode = viewModel.WorkTypeCode,
                    WorkTypeIcon = viewModel.Image.ConvertToByteArray(),
                    // WorkTypeColor = viewModel.WorkTypeColor,
                    Comment = viewModel.Comment,
                    CreatorId = User.Id,
                    CreateTime = DateTime.Now,
                    IsDeleted = false
                };
                _workTypeBl.Add(workType);
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
        [HttpGet]
        [DisplayName("ویرایش گروه کاربران")]
        //[IRITAuthorize(AssignableToRolePermissions.CanEditUser)]
        public virtual ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var workType = _workTypeBl.GetById((long)id);
            if (workType == null) return HttpNotFound();

            var viewModel = new WorkTypeViewModel
            {
                Id = workType.Id,
                WorkTypeTitle = workType.WorkTypeTitle,
                WorkTypeCode = workType.WorkTypeCode,
                //WorkTypeColor = workType.WorkTypeColor,
                Comment = workType.Comment
            };

            if (workType.WorkTypeIcon != null)
            {
                viewModel.ImageByte = workType.WorkTypeIcon;
            }
            //FillSelectList(null, null);
            return View(viewModel);

        }

        [HttpPost]
        [Route("Edit/{id}")]
        [ValidateAntiForgeryToken]
        //[IRITAuthorize(AssignableToRolePermissions.CanEditUser)]
        public virtual ActionResult Edit(WorkTypeViewModel viewModel)
        {
            var workType = _workTypeBl.GetById(viewModel.Id);
            workType.WorkTypeTitle = viewModel.WorkTypeTitle;
            workType.WorkTypeCode = viewModel.WorkTypeCode;
            //workType.WorkTypeColor = viewModel.WorkTypeColor;
            workType.Comment = viewModel.Comment;

            if (viewModel.Image != null)
            {
                workType.WorkTypeIcon = viewModel.Image.ConvertToByteArray();
            }

            _workTypeBl.Edit(workType);
            _dbContext.SaveAllChanges();

            this.NotySuccess(GeneralResource.Edit_successful);
            return RedirectToAction(MVC.Workflow.WorkType.ActionNames.Index, MVC.Workflow.WorkType.Name);
        }
        #endregion

        #region Delete
        public virtual JsonResult Delete(long id)
        {
            var workType = _workTypeBl.GetById(id);
            workType.IsDeleted = true;
            _workTypeBl.Edit(workType);
            _dbContext.SaveAllChanges();
            return Json(new { Message = "" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}