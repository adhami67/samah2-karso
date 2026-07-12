using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace IRIT.EdMS.Security.Model.ViewModel.User
{
    public class UserViewModel
    {
        #region ID
        public long Id { get; set; }
        #endregion

        #region PartyID
        [Required(ErrorMessage = "لطفا شخص را انتخاب کنید")]
        [DisplayName("انتخاب شخص")]
        public long? PartyId { get; set; }
        #endregion

        [Required(ErrorMessage = "لطفا نام خود را وارد کنید")]
        [DisplayName("نام")]
        [StringLength(256, ErrorMessage = "نام نباید کمتر از 3 حرف و بیتشر از 256 حرف باشد", MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "لطفا نام خانوادگی خود را وارد کنید")]
        [DisplayName("نام خانوادگی")]
        [StringLength(256, ErrorMessage = "نام خانوادگی نباید کمتر از 3 حرف و بیتشر از 256 حرف باشد", MinimumLength = 3)]
        public string LastName { get; set; }

        #region UserName
        [Required(ErrorMessage = "لطفا نام کاربری را وارد کنید")]
        [DisplayName("نام کاربری")]
        [StringLength(256, ErrorMessage = "کلمه عبور نباید کمتر از 3 حرف و بیتشر از 256 حرف باشد", MinimumLength = 3)]
        [Remote("IsUserNameExist", "User", "Security", ErrorMessage = "این نام کاربری قبلا در سیستم ثبت شده است", HttpMethod = "POST")]
        public string UserName { get; set; }
        #endregion

        #region UserPassword
        [Required(ErrorMessage = "لطفا کلمه عبور را وارد کنید")]
        [StringLength(50, ErrorMessage = "کلمه عبور نباید کمتر از 5 حرف و بیتشر از 50 حرف باشد", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [DisplayName("کلمه عبور")]
        public string UserPassword { get; set; }
        #endregion

        #region ReUserPassword
        [Required(ErrorMessage = "لطفا تکرار کلمه عبور را وارد کنید")]
        [StringLength(50, ErrorMessage = "کلمه عبور نباید کمتر از 5 حرف و بیتشر از 50 حرف باشد", MinimumLength = 5)]
        [System.ComponentModel.DataAnnotations.Compare("UserPassword", ErrorMessage = "لطفا کلمه عبور یکسان وارد کنید")]
        [DataType(DataType.Password)]
        [DisplayName("تکرار کلمه عبور")]
        public string ReUserPassword { get; set; }
        #endregion

        #region UserGroupIds
        public long[] UserGroupIds { get; set; }
        #endregion
    }
}
