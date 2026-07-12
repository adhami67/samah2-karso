using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using IRIT.Framework.Common.Enum;
using IRIT.Framework.Resources;

namespace IRIT.EdMS.Common.Model.ViewModel.Party
{
    public class PartyViewModel
    {
        #region Id
        public long Id { get; set; }
        #endregion

        #region FirstName
        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(GeneralResource))]
        [Display(Name = "FirstName", ResourceType = typeof(GeneralResource))]
        [StringLength(256, MinimumLength = 3, ErrorMessageResourceType = typeof(GeneralResource), ErrorMessageResourceName = "StringLength3To256")]
        public string FirstName { get; set; }
        #endregion

        #region LastNme
        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(GeneralResource))]
        [Display(Name = "LastName", ResourceType = typeof(GeneralResource))]
        [StringLength(256, MinimumLength = 3, ErrorMessageResourceType = typeof(GeneralResource), ErrorMessageResourceName = "StringLength3To256")]
        public string LastName { get; set; }
        #endregion

        #region NickName
        [Display(Name = "NickName", ResourceType = typeof(GeneralResource))]
        public string NickName { get; set; }
        #endregion

        #region father
        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(GeneralResource))]
        [Display(Name = "Father", ResourceType = typeof(GeneralResource))]
        [StringLength(256, MinimumLength = 3, ErrorMessageResourceType = typeof(GeneralResource), ErrorMessageResourceName = "StringLength3To256")]
        public string FatherName { get; set; }
        #endregion

        #region IDNumber
        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(GeneralResource))]
        [Display(Name = "IDNumber", ResourceType = typeof(GeneralResource))]
        [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceName = "JustNumber", ErrorMessageResourceType = typeof(GeneralResource))]
        [StringLength(10, MinimumLength = 1, ErrorMessageResourceType = typeof(GeneralResource), ErrorMessageResourceName = "StringLength1To10")]
        public string IdNumber { get; set; }
        #endregion

        #region   IDSerial
        [Display(Name = "IDSerial", ResourceType = typeof(GeneralResource))]
        [StringLength(256, MinimumLength = 8, ErrorMessageResourceType = typeof(GeneralResource), ErrorMessageResourceName = "StringLength8To256")]
        public string IdSerial { get; set; }
        #endregion

        #region Email
        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(GeneralResource))]
        [EmailAddress(ErrorMessageResourceName = "EmailCorrectly", ErrorMessageResourceType = typeof(GeneralResource))]
        [Display(Name = "Email", ResourceType = typeof(GeneralResource))]
        [StringLength(256, MinimumLength = 1, ErrorMessageResourceType = typeof(GeneralResource), ErrorMessageResourceName = "StringLength256")]
        [Remote("IsEmailExist", "Party", "Common", ErrorMessageResourceName = "EmialUsed", ErrorMessageResourceType = typeof(GeneralResource), HttpMethod = "POST")]
        public string Email { get; set; }
        #endregion

        #region NationalCode
        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(GeneralResource))]
        [Display(Name = "NationalCode", ResourceType = typeof(GeneralResource))]
        [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceName = "JustNumber", ErrorMessageResourceType = typeof(GeneralResource))]
        [StringLength(10, MinimumLength = 10, ErrorMessageResourceType = typeof(GeneralResource), ErrorMessageResourceName = "StringLength10")]
        [Remote("IsValidNationalCode", "Party", "Common", ErrorMessageResourceName = "InvalidNationalCode", ErrorMessageResourceType = typeof(GeneralResource), HttpMethod = "POST")]
        public string NationalCode { get; set; }
        #endregion

        #region Gender
        [Display(Name = "Gender", ResourceType = typeof(GeneralResource))]
        public GenderTypesEnum? Gender { get; set; }
        #endregion

        #region Nationality
        [Display(Name = "Nationality", ResourceType = typeof(GeneralResource))]
        public long? Nationality { get; set; }
        #endregion

        #region Country of birth
        [Display(Name = "CountryOfBirth", ResourceType = typeof(GeneralResource))]
        public long? BirthPlaceCountryId { get; set; }
        #endregion

        #region Province of birth
        [Display(Name = "ProvinceOfBirth", ResourceType = typeof(GeneralResource))]
        public long? BirthPlaceProvinceId { get; set; }
        #endregion

        #region City of birth
        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(GeneralResource))]
        [Display(Name = "CityOfBirth", ResourceType = typeof(GeneralResource))]
        public long? BirthPlaceCityId { get; set; }
        #endregion

        #region MobileNo
        [Display(Name = "MobileNo", ResourceType = typeof(GeneralResource))]
        [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceName = "JustNumber", ErrorMessageResourceType = typeof(GeneralResource))]
        public string MobileNo { get; set; }
        #endregion

        #region PhoneNo
        [Display(Name = "PhoneNo", ResourceType = typeof(GeneralResource))]
        [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceName = "JustNumber", ErrorMessageResourceType = typeof(GeneralResource))]
        public string PhoneNo { get; set; }
        #endregion

        #region Image
        public HttpPostedFileBase Image { get; set; }
        public byte[] ImageByte { get; set; }
        #endregion

        #region BirthDate
        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(GeneralResource))]

        [Display(Name = "BirthDate", ResourceType = typeof(GeneralResource))]
        [UIHint("PerDatePicker")]
        public DateTime BirthDate { get; set; }
        #endregion
    }
}
