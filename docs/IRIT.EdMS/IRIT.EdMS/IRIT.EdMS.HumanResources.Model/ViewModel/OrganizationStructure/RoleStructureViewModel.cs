using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IRIT.EdMS.HumanResource.Model.ViewModel.OrganizationStructure
{
    public class RoleStructureViewModel
    {
        [Key]
        public long Id { get; set; }

        [Required(ErrorMessage = "لطفا نام نقش را وارد کنید")]
        [DisplayName("نام نقش")]
        [StringLength(256, ErrorMessage = "نام نقش نباید کمتر از 3 حرف و بیتشر از 256 حرف باشد", MinimumLength = 3)]
        public string Title { get; set; }

        [DisplayName("کد نقش")]
        public string Code { get; set; }

        [Required(ErrorMessage = "لطفا تاریخ شروع به کار را وارد کنید")]
        [DisplayName("شروع به کار")]
        [UIHint("DatePicker")]
        public DateTime DatePicker { get; set; }

        [DisplayName("توضیحات")]
        public string Comment { get; set; }

        [Required(ErrorMessage = "لطفا ظرفیت را وارد کنید")]
        [DisplayName("ظرفیت")]
        public int Capacity { get; set; }

        public long? ParentId { get; set; }

        public DateTime EnglishDateTime => DateTime.Parse($"{DatePicker.Date.Year}-{DatePicker.Date.Month}-{DatePicker.Date.Day}");
        //public DateTime EnglishDateTime => new Framework.Utility.Utility.PersianDateTime(DatePicker.Year, DatePicker.Month, DatePicker.Day).EnglishDateTime;
    }
}
