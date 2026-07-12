using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using IRIT.Framework.DataAccess.Context;
using System.Web.Security;
using System.Web.UI.WebControls;
using IRIT.EdMS.Security.Business;
using IRIT.EdMS.Security.Contracts;
using IRIT.Framework.Resources;
using IRIT.Framework.Utility.Controller;
using IRIT.Framework.Utility.Security;
using IRIT.Framework.Utility.Utility;
using LoginStatus = IRIT.Framework.Common.Enum.LoginStatus;
using IRIT.EdMS.Security.Business.Contracts;
using IRIT.EdMS.Security.Business.Business;
using IRIT.EdMS.Security.Model.ViewModel.Account;

namespace IRIT.EdMS.WebUI.Areas.Security.Controllers
{
    public partial class AccountController : BaseController
    {
        #region Data Member
        private readonly ApplicationDbContext _dbContext;
        private readonly IUsers _userBl;
        // private readonly IAuthenticationManager _authenticationManager;
        #endregion

        #region Constractor
        public AccountController()
        {
            _dbContext = new ApplicationDbContext();
            _userBl = new UsersBl(_dbContext);
            //_authenticationManager = HttpContext.GetOwinContext().Authentication;
        }
        #endregion

        #region Login,LogOff
        [AllowAnonymous]
        public virtual ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        //[CheckReferrer]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Login(LoginViewModel model, string returnUrl)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            var userLogin = _userBl.UserLogin(model.UserName.ToLower(), PasswordSecurity.AdvancePasswordHash(model.Password));

            switch (userLogin.Status)
            {
                case LoginStatus.Success:
                    DoLogin(model.UserName.ToLower(), model.RememberMe, userLogin);

                    return RedirectToLocal(returnUrl);

                case LoginStatus.RequiresVerification:
                    ModelState.AddModelError(string.Empty, GeneralResource.IncorrectUserPass);
                    return View(model);

                case LoginStatus.Failure:
                    ModelState.AddModelError(string.Empty, GeneralResource.PossibilityEntering);
                    return View(model);

                default:
                    ModelState.AddModelError(string.Empty, GeneralResource.PossibilityEntering);
                    return View(model);
            }
        }

        private void DoLogin(string userName, bool rememberMe, AccountDetailsViewModel userLogin)
        {
            var serializeModel = new EdMSPrincipalSerializeModel
            {
                Id = userLogin.UserId,
                FullName = userLogin.FullName,
                UserName = userName,
                PartyId = userLogin.PartyId,
                SchoolId = userLogin.SchoolId ?? 0
            };

            //if (userLogin.SmallImage != null)
            //{
            //    var ff = File(userLogin.SmallImage, "image/jpeg");

            //    var base64 = Convert.ToBase64String(userLogin.SmallImage);
            //   // serializeModel.UserSmallImage = $"data:image/jpg;base64,{base64}";
            //}
            //else
            //{
            //   // serializeModel.UserSmallImage = "../../../Content/Image/Icon/default_avatar.png";
            //}

            if (userLogin.SchoolId != null) serializeModel.SchoolId = userLogin.SchoolId.Value;

            var serializer = new JavaScriptSerializer();

            var userData = serializer.Serialize(serializeModel);

            var authTicket = new FormsAuthenticationTicket(
                     1,
                     userName,
                     DateTime.Now,
                     DateTime.Now.AddMinutes(15),
                     rememberMe,
                     userData);

            var encTicket = FormsAuthentication.Encrypt(authTicket);
            var faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            Response.Cookies.Add(faCookie);
        }

        public virtual ActionResult RenderPhoto(long userId)
        {
            //var photo = _userBl.GetById(userId).Party.SmallImage;

            //if (photo != null)
            //    return File(photo, "image/jpeg");

            var avatarAddress = "~/Content/Image/Icon/default_avatar.png";
           var photo = new WebClient().DownloadData(avatarAddress.ToAbsoluteUrl());

            return File(photo, "image/jpeg");
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public virtual ActionResult LogOff()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();

            return RedirectToAction(MVC.Security.Account.ActionNames.Login, MVC.Security.Account.Name, new { area = MVC.Security.Name });
        }

        #endregion

        #region ResePassword

        //[AllowAnonymous]
        //public virtual ActionResult ResetPassword(string code)
        //{
        //    //if(enable resetpass feature then show resetpass page)
        //    //return view("info")
        //    if (code == null) return HttpNotFound();

        //    return View();
        //}

        //[HttpPost]
        //[AllowAnonymous]
        ////[CheckReferrer]
        //[ValidateAntiForgeryToken]
        //[CaptchaVerify("تصویر امنیتی را درست وارد کنید")]
        //public virtual async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        //{
        //    if (!model.Password.IsSafePasword())
        //        this.AddErrors("Password", "این کلمه عبور به راحتی قابل تشخیص است");
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }
        //    var user = await _userManager.FindByNameAsync(model.Email.ToLower());
        //    if (user == null)
        //    {
        //        // Don't reveal that the user does not exist
        //        return RedirectToAction(MVCApp.Account.ActionNames.ResetPasswordConfirmation, MVCApp.Account.Name);
        //    }
        //    var result = await _userManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
        //    if (result.Succeeded)
        //    {
        //        await _signInManager.SignInAsync(user, false, false);
        //        return RedirectToAction(MVCApp.Account.ActionNames.ResetPasswordConfirmation, MVCApp.Account.Name);
        //    }
        //    this.AddErrors(result);
        //    this.NotyError(ModelState.GetListOfErrors());
        //    return View(model);
        //}

        //[AllowAnonymous]
        //public virtual ActionResult ResetPasswordConfirmation()
        //{
        //    return View();
        //}
        #endregion

        #region Validation

        //[HttpPost]
        //[AllowAnonymous]
        //[OutputCache(Location = OutputCacheLocation.None, NoStore = true, Duration = 0, VaryByParam = "*")]
        //public virtual JsonResult IsEmailAvailable(string email)
        //{
        //    return _userManager.IsEmailAvailableForConfirm(email) ? Json(true) : Json(false);
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //[OutputCache(Location = OutputCacheLocation.None, NoStore = true, Duration = 0, VaryByParam = "*")]
        //public virtual JsonResult CheckPassword(string password)
        //{
        //    return password.IsSafePasword() ? Json(true) : Json(false);
        //}
        //[HttpPost]
        //[AllowAnonymous]
        //[OutputCache(Location = OutputCacheLocation.None, NoStore = true, Duration = 0, VaryByParam = "*")]
        //public virtual JsonResult IsNameForShowExist(string nameForShow, int? id)
        //{
        //    return _userManager.CheckNameForShowExist(nameForShow, id) ? Json(false) : Json(true);
        //}
        //[HttpPost]
        //[AllowAnonymous]
        //[OutputCache(Location = OutputCacheLocation.None, NoStore = true, Duration = 0, VaryByParam = "*")]
        //public virtual JsonResult IsEmailExist(string email, int? id)
        //{
        //    var check = _userManager.CheckEmailExist(email, id);
        //    return check ? Json(false) : Json(true);
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //[OutputCache(Location = OutputCacheLocation.None, NoStore = true, Duration = 0, VaryByParam = "*")]
        //public virtual JsonResult IsUserNameExist(string userName, int? id)
        //{
        //    return _userManager.CheckUserNameExist(userName, id) ? Json(false) : Json(true);
        //}
        #endregion
    }
}