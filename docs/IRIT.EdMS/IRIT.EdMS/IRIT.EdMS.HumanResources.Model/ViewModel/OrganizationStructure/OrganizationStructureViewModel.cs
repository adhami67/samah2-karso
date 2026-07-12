using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IRIT.EdMS.HumanResource.Model.ViewModel.OrganizationStructure
{
    public class OrganizationStructureViewModel
    {
        [Key]
        public long Id { get; set; }

        [Required(ErrorMessage = "لطفا نام واحد را وارد کنید")]
        [DisplayName("نام واحد")]
        [StringLength(256, ErrorMessage = "نام واحد نباید کمتر از 3 حرف و بیتشر از 256 حرف باشد", MinimumLength = 3)]
        public string Title { get; set; }

        [DisplayName("کد واحد")]
        public string Code { get; set; }

        [Required(ErrorMessage = "لطفا تاریخ شروع به کار را وارد کنید")]
        [DisplayName("شروع به کار")]
        [UIHint("DatePicker")]
        public DateTime DatePicker { get; set; }

        [DisplayName("توضیحات")]
        public string Comment { get; set; }

        public long? ParentId { get; set; }
    }
}
