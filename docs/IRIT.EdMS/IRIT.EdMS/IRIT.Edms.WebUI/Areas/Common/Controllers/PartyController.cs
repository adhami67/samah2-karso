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
using IRIT.EdMS.Common.Model.ViewModel.Party;
using IRIT.Framework.DataModel.Common;
using IRIT.Framework.Resources;
using IRIT.Framework.Utility.Filters;
using IRIT.Framework.Utility.Utility;
using IRIT.Framework.Common.Enum;
#endregion

namespace IRIT.EdMS.WebUI.Areas.Common.Controllers
{
    [Authorize]
    public partial class PartyController : BaseController
    {
        #region Data Member
        private ApplicationDbContext _dbContext { get; }
        private readonly IParty _partyBl;
        private readonly IRegion _regionBl;
        private readonly IPartyOwner _partyOwnerBl;
        private readonly ISchool _schoolBl;
        #endregion

        #region Constractor
        public PartyController()
        {
            _dbContext = new ApplicationDbContext();
            _partyBl = new PartyBlo(_dbContext);
            _regionBl = new RegionBl(_dbContext);
            _partyOwnerBl = new PartyOwnerBl(_dbContext);
            _schoolBl = new SchoolBl(_dbContext);
        }
        #endregion

        #region Get
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult Party_Read([DataSourceRequest]DataSourceRequest request)
        {
            var partys = _partyBl.GetAll();
            return Json(partys.ToDataSourceResult(request));
        }
        #endregion

        #region Create

        public virtual ActionResult Create()
        {
            FillSelectList(null, null);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "Create")]
        //[IRITAuthorize(AssignableToRolePermissions.CanCreateUser)]
        public virtual ActionResult Create(PartyViewModel viewModel)
        {
            #region Validation

            #endregion

            if (InsertParty(viewModel))
            {
                this.NotySuccess(GeneralResource.Add_successful);
                return RedirectToAction(MVC.Common.Party.ActionNames.Index, MVC.Common.Party.Name);
            }
            this.NotyError(GeneralResource.ErrorInRequest, true);
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "Create_new")]
        public virtual ActionResult Create_new(PartyViewModel viewModel)
        {
            #region Validation

            #endregion

            if (InsertParty(viewModel))
            {
                this.NotySuccess(GeneralResource.Add_successful);
                return RedirectToAction(MVC.Common.Party.ActionNames.Create, MVC.Common.Party.Name);
            }
            this.NotyError(GeneralResource.ErrorInRequest, true);
            return View("Create", viewModel);
        }

        private bool InsertParty(PartyViewModel viewModel)
        {
            try
            {
                var party = new Party
                {
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    //SmallImage = viewModel.Image != null ? viewModel.Image.ConvertToByteArray() : (byte?[]) null,
                    CreatorId = User.Id,
                    CreateTime = DateTime.Now,
                    IsDeleted = false,
                    BirthDate = viewModel.BirthDate,
                    Email = viewModel.Email,
                    BirthPlaceId = viewModel.BirthPlaceCityId,
                    FatherName = viewModel.FatherName,
                    PhoneNo = viewModel.PhoneNo,
                    MobileNo = viewModel.MobileNo,
                    NickName = viewModel.NickName,
                    NationalityId = viewModel.Nationality,
                    IdNumber = viewModel.IdNumber,
                    IdSerial = viewModel.IdSerial,
                    NationalCode = viewModel.NationalCode,
                    GenderType = viewModel.Gender.Value
                };
                _partyBl.Add(party);
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
        //[Route("Edit/{id}")]
        [HttpGet]
        [DisplayName("ویرایش گروه کاربران")]
        //[IRITAuthorize(AssignableToRolePermissions.CanEditUser)]
        public virtual ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var party = _partyBl.GetById((long)id);
            if (party == null) return HttpNotFound();

            var viewModel = new PartyViewModel
            {
                Id = party.Id,
                FirstName = party.FirstName,
                LastName = party.LastName,
                //BirthDate = null,
                Email = party.Email,
                //Todo SmallImage =                 
                FatherName = party.FatherName,
                PhoneNo = party.PhoneNo,
                MobileNo = party.MobileNo,
                NickName = party.NickName,
                Nationality = party.NationalityId,
                IdNumber = party.IdNumber,
                IdSerial = party.IdSerial,
                NationalCode = party.NationalCode,
                //Gender = (byte?)party.GenderType
            };

            if (party.SmallImage != null)
            {
                //viewModel.ImageByte = party.SmallImage;
            }

            if (party.BirthPlaceId.HasValue)
            {
                var provinceId = _regionBl.GetById(party.BirthPlaceId.Value).ParentId;
                var countryId = _regionBl.GetById(provinceId.Value).ParentId;

                viewModel.BirthPlaceCountryId = countryId;
                viewModel.BirthPlaceProvinceId = provinceId;
                viewModel.BirthPlaceCityId = party.BirthPlaceId;

                FillSelectList(countryId, provinceId);
                return View(viewModel);
            }
            else
            {
                FillSelectList(null, null);
                return View(viewModel);
            }
        }

        [HttpPost]
        [Route("Edit/{id}")]
        [ValidateAntiForgeryToken]
        //[IRITAuthorize(AssignableToRolePermissions.CanEditUser)]
        public virtual ActionResult Edit(PartyViewModel viewModel)
        {
            var party = _partyBl.GetById(viewModel.Id);
            party.FirstName = viewModel.FirstName;
            party.LastName = viewModel.LastName;
            // party.BirthDate = null;
            party.Email = viewModel.Email;
            // Todo SmallImage =                 
            party.FatherName = viewModel.FatherName;
            party.PhoneNo = viewModel.PhoneNo;
            party.MobileNo = viewModel.MobileNo;
            party.NickName = viewModel.NickName;
            party.NationalityId = viewModel.Nationality;
            party.IdNumber = viewModel.IdNumber;
            party.IdSerial = viewModel.IdSerial;
            party.NationalCode = viewModel.NationalCode;
            //party.GenderType = viewModel.Gender;

            if (viewModel.Image != null)
            {
                //party.SmallImage = viewModel.Image.ConvertToByteArray();
            }

            _partyBl.Edit(party);
            _dbContext.SaveAllChanges();

            this.NotySuccess(GeneralResource.Edit_successful);
            return RedirectToAction(MVC.Common.Party.ActionNames.Index, MVC.Common.Party.Name);
        }
        #endregion

        #region Delete
        public virtual JsonResult Delete(long id)
        {
            var party = _partyBl.GetById(id);
            party.IsDeleted = true;
            _partyBl.Edit(party);
            _dbContext.SaveAllChanges();
            return Json(new { Message = "" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region RemoteValidations
        [HttpPost]
        [AllowAnonymous]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true, Duration = 0, VaryByParam = "*")]
        public virtual JsonResult IsEmailExist(string email, int? id)
        {
            var check = _partyBl.CheckEmailExist(email, id);
            return check ? Json(false) : Json(true);
        }

        [HttpPost]
        [AllowAnonymous]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true, Duration = 0, VaryByParam = "*")]
        public virtual JsonResult IsValidNationalCode(string nationalCode)
        {
            return Framework.Common.Validations.NationalCodeValidation.IsValidNationalCode(nationalCode) ? Json(true) : Json(false);
        }

        public virtual JsonResult ChangeCountryComboBox(long id)
        {
            var Province = new List<object> { new { ID = 0, Title =  GeneralResource.Unknown} };
            Province.AddRange(_regionBl.Get(c => c.RegionType == (long)RegionKindEnum.Province && c.ParentId == id).Select(c => new {c.Id, c.Title }));
            return Json(Province, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult ChangeProvinceComboBox(long id)
        {
            var citys = _regionBl.Get(c => c.RegionType == (long)RegionKindEnum.City && c.ParentId == id).Select(c => new {c.Id, c.Title });
            return Json(citys, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Private
        [NonAction]
        private void FillSelectList(long? countryId, long? provinceId)
        {
            #region PartyOwner
            var partyOwners = new List<SelectListItem>();

            //partyOwners.AddRange(User.Id == 1
            //    ? _schoolBl.GetAll().Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.SchoolName })
            //    : _partyOwnerBl.Get(c => c.PartyId == User.PartyId)
            //        .Select(c => new SelectListItem {Value = c.OwnerId.ToString(), Text = c.OwnerTitle}));

            ViewBag.PartyOwners = partyOwners;
            #endregion

            #region Gender
            ViewBag.Genders = new SelectList(Framework.Common.Enum.GenderTypes.GenderTypesDic, "Key", "Value");
            #endregion

            #region Nationality
            ViewBag.Nationalitys = new SelectList(_regionBl.Get(c => c.RegionType == (long)RegionKindEnum.Country), "ID", "Title");
            #endregion

            #region Country
            var countrys = new List<Region> { new Region { Id = 0, Title = GeneralResource.Unknown} };
            countrys.AddRange(_regionBl.Get(c => c.RegionType == (long)RegionKindEnum.Country));
            ViewBag.BirthDateNationalitys = new SelectList(countrys, "ID", "Title");
            #endregion

            #region Province
            var contId = countryId.HasValue ? countryId.Value : 0;
            var prov = _regionBl.Get(c => c.RegionType == (long)RegionKindEnum.Province && c.ParentId == contId);
            ViewBag.Provinces = new SelectList(prov, "ID", "Title");
            #endregion

            #region City
            var prcId = provinceId.HasValue ? provinceId.Value : 0;
            ViewBag.Citys = new SelectList(_regionBl.Get(c => c.RegionType == (long)RegionKindEnum.City && c.ParentId == prcId), "ID", "Title");
            #endregion
        }
        #endregion
    }
}