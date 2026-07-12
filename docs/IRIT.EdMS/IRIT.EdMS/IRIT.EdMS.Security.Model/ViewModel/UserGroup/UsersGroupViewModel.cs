using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IRIT.EdMS.Security.Model.ViewModel.UserGroup
{
    public class UsersGroupViewModel
    {
        [Key]
        public long Id { get; set; }

        [Required(ErrorMessage = "لطفا نام گروه کاربر را وارد کنید")]
        [DisplayName("نام گروه کاربر")]
        [StringLength(256, ErrorMessage = "نام گروه کاربر نباید کمتر از 3 حرف و بیتشر از 256 حرف باشد", MinimumLength = 3)]
        public string GroupName { get; set; }

        [DisplayName("توضیحات")]
        public string Comment { get; set; }
    }
}
