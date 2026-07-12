#region Using
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
using WebGrease.Css.Extensions;
using AutoMapper;
using IRIT.Framework.DataModel;
using IRIT.EdMS.Common.Business;
using IRIT.EdMS.Common.Business.Business;
using IRIT.EdMS.Common.Business.Contract;
using IRIT.EdMS.Security.Business.Business;
using IRIT.EdMS.Security.Business.Contracts;
using IRIT.EdMS.Security.Model.ViewModel.User;
using IRIT.Framework.DataModel.Security;
using IRIT.Framework.Resources;

#endregion

namespace IRIT.EdMS.WebUI.Areas.Security.Controllers
{
    [Authorize]
    public partial class UserController : BaseController
    {
        #region Data Member
        private ApplicationDbContext DbContext { get; }
        private readonly IUsers _userBl;
        private readonly IUsersGroup _userGroupBl;
        private readonly IParty _partyBl;
        #endregion

        #region Constractor
        public UserController()
        {
            DbContext = new ApplicationDbContext();
            _userBl = new UsersBl(DbContext);
            _userGroupBl = new UsersGroupBl(DbContext);
            _partyBl = new PartyBlo(DbContext);
        }
        #endregion

        #region Get
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult Users_Read([DataSourceRequest]DataSourceRequest request)
        {
            var users = _userBl.GetAll();
            return Json(users.ToDataSourceResult(request));
        }
        #endregion

        #region Create

        public virtual ActionResult Create()
        {
            PopulateUsersGroups();
            FillSelectList(null);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "Create")]
        //[IRITAuthorize(AssignableToRolePermissions.CanCreateUser)]
        public virtual ActionResult Create(UserViewModel viewModel)
        {
            #region Validation
            if (_userBl.CheckUserNameExist(viewModel.UserName, null))
                this.AddErrors("UserName", "این نام کاربری قبلا در سیستم ثبت شده است");
            if (!ModelState.IsValid)
            {
                PopulateUsersGroups(viewModel.UserGroupIds);
                return View(viewModel);
            }
            #endregion

           

            if (InsertUser(viewModel))
            {
                this.NotySuccess(GeneralResource.Add_successful);
                return RedirectToAction(MVC.Security.User.ActionNames.Index, MVC.Security.User.Name);
            }
            this.NotyError(GeneralResource.ErrorInRequest, true);
            FillSelectList(null);
            return View( viewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "Create_new")]
        //[IRITAuthorize(AssignableToRolePermissions.CanCreateUser)]
        public virtual ActionResult Create_new(UserViewModel viewModel)
        {
            #region Validation
            if (_userBl.CheckUserNameExist(viewModel.UserName, null))
                this.AddErrors("UserName", "این نام کاربری قبلا در سیستم ثبت شده است");
            if (!ModelState.IsValid)
            {
                PopulateUsersGroups(viewModel.UserGroupIds);
                return View("Create",viewModel);
            }
            #endregion



            if (InsertUser(viewModel))
            {
                this.NotySuccess(GeneralResource.Add_successful);
                return RedirectToAction(MVC.Security.User.ActionNames.Create, MVC.Security.User.Name);
            }
            this.NotyError(GeneralResource.ErrorInRequest, true);
            FillSelectList(null);
            return View("Create", viewModel);

        }
        private bool InsertUser(UserViewModel viewModel)
        {
            try
            {
                var user = new User
                {
                    CreatorId = User.Id,
                    CreateTime = DateTime.Now,
                    PartyId = viewModel.PartyId.Value,
                    UserName = viewModel.UserName,
                    UserPassword =
                        Framework.Utility.Security.PasswordSecurity.AdvancePasswordHash(viewModel.UserPassword)
                };
                _userBl.Add(user);
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
        [Route("Edit/{id}")]
        [HttpGet]
        [DisplayName("ویرایش کاربر")]
        //[IRITAuthorize(AssignableToRolePermissions.CanEditUser)]
        [ActivityLog(Name = "EditUser", Description = "ویرایش کاربر")]
        public virtual ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = _userBl.GetById((long)id);

            if (user == null) return HttpNotFound();

            PopulateUsersGroups(null);
            FillSelectList(user.PartyId);
            var viewModel = new UserViewModel { Id = user.Id, ReUserPassword = user.UserPassword, UserName = user.UserName, UserPassword = user.UserPassword, PartyId = user.PartyId };
            return View(viewModel);
        }
        
        [HttpPost]
        [Route("Edit/{id}")]
        [ValidateAntiForgeryToken]
        //[IRITAuthorize(AssignableToRolePermissions.CanEditUser)]
        public virtual ActionResult Edit(UserViewModel viewModel)
        {
            #region Validation
            if (_userBl.CheckUserNameExist(viewModel.UserName, viewModel.Id))
                this.AddErrors("UserName", "این نام کاربری قبلا در سیستم ثبت شده است");
            #endregion

            this.NotySuccess(GeneralResource.Edit_successful);
            return RedirectToAction(MVC.Security.User.ActionNames.Index, MVC.Security.User.Name);
        }
        #endregion

        #region Delete
        public virtual JsonResult Delete(long id)
        {
            return Json(new { Message = "" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region RemoteValidations
        [HttpPost]
        [AllowAnonymous]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true, Duration = 0, VaryByParam = "*")]
        public virtual JsonResult IsUserNameExist(string userName, int? id)
        {
            return _userBl.CheckUserNameExist(userName, id) ? Json(false) : Json(true);
        }
        #endregion

        #region Private
        [NonAction]
        private void PopulateUsersGroups(params long[] selectedIds)
        {
            var usersGroups = _userGroupBl.GetAllAsSelectList();

            if (selectedIds != null)
            {
                usersGroups.ForEach(a => a.Selected = selectedIds.Any(b => long.Parse(a.Value) == b));
            }

            ViewBag.UsersGroups = usersGroups;
        }

        private void FillSelectList(long? partyID)
        {
            //var partys = new List<object>();
            //if(partyID.HasValue)
            //{
            //    var ret = _partyBl.Get(partyID.Value);
            //    partys.Add(new { Id = ret.ID, FullName = $"{ret.FirstName} {ret.LastName}" });
            //}
            //partys.AddRange(_partyBl.GetViewPartyUser(c => c.SchoolID == User.SchoolId).Select(c => new { Id = c.ID, FullName = $"{c.FirstName} {c.LastName}" }));
            //ViewBag.Partys = new SelectList(partys, "Id", "FullName");
        }
        #endregion
    }
}