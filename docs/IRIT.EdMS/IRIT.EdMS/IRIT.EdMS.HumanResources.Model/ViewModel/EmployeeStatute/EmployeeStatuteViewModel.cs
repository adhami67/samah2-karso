using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IRIT.EdMS.HumanResource.Model.ViewModel.EmployeeStatute
{
    public class EmployeeStatuteViewModel
    {
        #region ID
        public long Id { get; set; }
        #endregion

        #region PartyID
        [Required(ErrorMessage = "لطفا کارمند را انتخاب کنید")]
        [DisplayName("انتخاب کارمند")]
        public long? EmployeeId { get; set; }
        #endregion

        #region EmploymentNumber
        [Required(ErrorMessage = "لطفا شماره کارمندي را وارد کنید")]
        [DisplayName("شماره کارمندي")]
        [StringLength(100, ErrorMessage = "شماره نباید کمتر از 3 حرف و بیتشر از 100 حرف باشد", MinimumLength = 3)]
        public string EmploymentNumber { get; set; }
        #endregion

        #region FirstName
        [Required(ErrorMessage = "لطفا نام خود را وارد کنید")]
        [DisplayName("نام")]
        [StringLength(256, ErrorMessage = "نام نباید کمتر از 3 حرف و بیتشر از 256 حرف باشد", MinimumLength = 3)]
        public string FirstName { get; set; }
        #endregion

        #region LastName
        [Required(ErrorMessage = "لطفا نام خانوادگی خود را وارد کنید")]
        [DisplayName("نام خانوادگی")]
        [StringLength(256, ErrorMessage = "نام خانوادگی نباید کمتر از 3 حرف و بیتشر از 256 حرف باشد", MinimumLength = 3)]
        public string LastName { get; set; }
        #endregion

        #region OrganizationStructureId
        public long OrganizationStructureId { get; set; }
        #endregion

        #region ApplyDate
        [DisplayName("تاریخ انتصاب")]
        [UIHint("DatePicker")]
        public DateTime ApplyDate { get; set; }
        #endregion

        #region Comment
        [DisplayName("توضیحات")]
        public string Comment { get; set; }
        #endregion
    }
}
