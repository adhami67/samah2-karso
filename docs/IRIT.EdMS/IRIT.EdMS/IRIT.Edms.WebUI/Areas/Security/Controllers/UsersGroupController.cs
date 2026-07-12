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
using IRIT.EdMS.Security.Business.Business;
using IRIT.EdMS.Security.Business.Contracts;
using IRIT.EdMS.Security.Model.ViewModel.UserGroup;
using IRIT.Framework.DataModel;
using IRIT.Framework.DataModel.Security;
using IRIT.Framework.Resources;

namespace IRIT.EdMS.WebUI.Areas.Security.Controllers
{
    [Authorize]
    public partial class UsersGroupController : BaseController
    {
        #region Data Member
        private ApplicationDbContext DbContext { get; }
        private readonly IUsersGroup _userGroupBl;
        #endregion

        #region Constractor
        public UsersGroupController()
        {
            DbContext = new ApplicationDbContext();
            _userGroupBl = new UsersGroupBl(DbContext);
        }
        #endregion

        #region Get
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult UsersGroup_Read([DataSourceRequest]DataSourceRequest request)
        {
            var users = _userGroupBl.GetAll();
            return Json(users.ToDataSourceResult(request));
        }
        #endregion

        #region Create

        public virtual ActionResult Create()
        {
            return View();
        }        

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Create")]
        [ValidateAntiForgeryToken]
        //[IRITAuthorize(AssignableToRolePermissions.CanCreateUser)]
        public virtual ActionResult Create(UsersGroupViewModel viewModel)
        {
            #region Validation
            if (!ModelState.IsValid)
            {
                this.NotyWarning(GeneralResource.RequiredMessage);
                return View(viewModel);
            }
            #endregion



            if (InsertUsersGroup(viewModel))
            {
                this.NotySuccess(GeneralResource.Add_successful);
                return RedirectToAction(MVC.Security.UsersGroup.ActionNames.Index, MVC.Security.UsersGroup.Name);
            }
            this.NotyError(GeneralResource.ErrorInRequest, true);
            return View( viewModel);

        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Create_new")]
        [ValidateAntiForgeryToken]
        //[IRITAuthorize(AssignableToRolePermissions.CanCreateUser)]
        public virtual ActionResult Create_new(UsersGroupViewModel viewModel)
        {
            #region Validation
            if (!ModelState.IsValid)
            {
                this.NotyWarning(GeneralResource.RequiredMessage);
                return View("Create",viewModel);
            }
            #endregion



            if (InsertUsersGroup(viewModel))
            {
                this.NotySuccess(GeneralResource.Add_successful);
                return RedirectToAction(MVC.Security.UsersGroup.ActionNames.Create, MVC.Security.UsersGroup.Name);
            }
            this.NotyError(GeneralResource.ErrorInRequest, true);
            return View("Create", viewModel);

        }
        private bool InsertUsersGroup(UsersGroupViewModel viewModel)
        {
            try
            {
                var userGroup = new UsersGroup
                {
                    GroupName = viewModel.GroupName,
                    CreatorId = User.Id,
                    CreateTime = DateTime.Now,
                    IsDeleted = false
                };
                _userGroupBl.Add(userGroup);
                DbContext.SaveAllChanges();
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
        [DisplayName("ویرایش گروه کاربران")]
        //[IRITAuthorize(AssignableToRolePermissions.CanEditUser)]
        [ActivityLog(Name = "EditUser", Description = "ویرایش گروه کاربران")]
        public virtual ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var userGroup = _userGroupBl.GetById((long)id);
            if (userGroup == null) return HttpNotFound();

            var viewModel = new UsersGroupViewModel { Id = userGroup.Id, GroupName = userGroup.GroupName};
            return View(viewModel);
        }

        [HttpPost]
        [Route("Edit/{id}")]
        [ValidateAntiForgeryToken]
        //[IRITAuthorize(AssignableToRolePermissions.CanEditUser)]
        public virtual ActionResult Edit(UsersGroupViewModel viewModel)
        {
            #region Validation
            if (!ModelState.IsValid)
            {
                this.NotyWarning("لطفا نام گروه کاربری را صحیح وارد کنید");
                return View(viewModel);
            }
            #endregion

            var userGroup = _userGroupBl.GetById(viewModel.Id);
            userGroup.GroupName = viewModel.GroupName;

            _userGroupBl.Edit(userGroup);
            DbContext.SaveAllChanges();

            this.NotySuccess("عملیات  ویرایش گروه کاربری با موفقیت انجام شد");
            return RedirectToAction(MVC.Security.UsersGroup.ActionNames.Index, MVC.Security.UsersGroup.Name);
        }
        #endregion

        #region Delete
        public virtual JsonResult Delete(long id)
        {
            _userGroupBl.Delete(id);
            DbContext.SaveAllChanges();
            return Json(new { Message = "" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region RemoteValidations

        #endregion

        #region Private
        
        #endregion
    }
}