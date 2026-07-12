using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using IRIT.Framework.Common.Enum;
using IRIT.Framework.Resources;

namespace IRIT.EdMS.Common.Model.ViewModel.School
{
    public class SchoolViewModel
    {
        #region SchoolsGroupID
        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(GeneralResource))]
        [Display(Name = "SchoolsGroupID", ResourceType = typeof(GeneralResource))]
        public long SchoolsGroupId { get; set; }
        #endregion

        #region Id
        public long Id { get; set; }

        #endregion

        #region Code
        [Display(Name = "Code", ResourceType = typeof(GeneralResource))]
        [StringLength(256, MinimumLength = 3, ErrorMessageResourceType = typeof(GeneralResource), ErrorMessageResourceName = "StringLength3To256")]
        public string Code { get; set; }
        #endregion

        #region SchoolName
        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(GeneralResource))]
        [Display(Name = "SchoolName", ResourceType = typeof(GeneralResource))]
        [StringLength(256, MinimumLength = 3, ErrorMessageResourceType = typeof(GeneralResource), ErrorMessageResourceName = "StringLength3To256")]
        public string SchoolName { get; set; }
        #endregion

        #region IssueDate
        [Display(Name = "IssueDate", ResourceType = typeof(GeneralResource))]
        [UIHint("PerDatePicker")]
        public DateTime? IssueYear { get; set; }
        #endregion

        #region Image
        public HttpPostedFileBase Image { get; set; }
        public byte[] ImageByte { get; set; }
        #endregion

        #region PostAddress
        [Display(Name = "PostAddress", ResourceType = typeof(GeneralResource))]
        public string PostAddress { get; set; }
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

        #region FaxNo
        [Display(Name = "FaxNo", ResourceType = typeof(GeneralResource))]
        [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceName = "JustNumber", ErrorMessageResourceType = typeof(GeneralResource))]
        public string FaxNo { get; set; }
        #endregion

        #region Email
        [EmailAddress(ErrorMessageResourceName = "EmailCorrectly", ErrorMessageResourceType = typeof(GeneralResource))]
        [Display(Name = "Email", ResourceType = typeof(GeneralResource))]
        [StringLength(256, MinimumLength = 1, ErrorMessageResourceType = typeof(GeneralResource), ErrorMessageResourceName = "StringLength256")]
        public string Email { get; set; }
        #endregion

        #region HomePageURL
        [Display(Name = "IDNumber", ResourceType = typeof(GeneralResource))]
        public string HomePageUrl { get; set; }
        #endregion

        #region Comment
        [Display(Name = "Comment", ResourceType = typeof(GeneralResource))]
        public string Comment { get; set; }
        #endregion

        #region Gender
        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(GeneralResource))]
        [Display(Name = "Gender", ResourceType = typeof(GeneralResource))]
        public GenderTypesEnum GenderKindId { get; set; }
        #endregion

        #region GradeKindID
        [Key]
        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(GeneralResource))]
        [Display(Name = "GradeKind", ResourceType = typeof(GeneralResource))]
        public long GradeKindId { get; set; }
        #endregion
    }
}
