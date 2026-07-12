using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IRIT.EdMS.HumanResource.Model.ViewModel.Employee
{
    public class EmployeeViewModel
    {
        #region ID
        public long Id { get; set; }
        #endregion

        #region PartyID
        [Required(ErrorMessage = "لطفا شخص را انتخاب کنید")]
        [DisplayName("انتخاب شخص")]
        public long? PartyId { get; set; }
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

        #region MilitaryServiceKind
        [Required(ErrorMessage = "لطفا وضعيت خدمت سربازي را انتخاب کنید")]
        [DisplayName("وضعيت خدمت سربازي")]
        public byte? MilitaryServiceKind { get; set; }
        #endregion

        #region MarriageStatusKind
        [Required(ErrorMessage = "لطفا وضعيت تاهل را انتخاب کنید")]
        [DisplayName("وضعيت تاهل")]
        public byte? MarriageStatusKind { get; set; }
        #endregion

        #region MilitaryServiceStartDate
        [Required(ErrorMessage = "لطفا از تاريخ را وارد کنید")]
        [UIHint("DatePicker")]
        [DisplayName("از تاريخ")]
        public DateTime MilitaryServiceStartDate { get; set; }
        #endregion

        #region MilitaryServiceFinishDate
        [Required(ErrorMessage = "لطفا تا تاريخ را وارد کنید")]
        [UIHint("DatePicker")]
        [DisplayName("تا تاريخ")]
        public DateTime MilitaryServiceFinishDate { get; set; }
        #endregion

        #region EmployementKindID
        [Required(ErrorMessage = "لطفا نوع استخدام را انتخاب کنید")]
        [DisplayName("نوع استخدام")]
        public byte? EmployementKindId { get; set; }
        #endregion

        #region Comment
        [DisplayName("توضیحات")]
        public string Comment { get; set; }
        #endregion

        #region FullName
        [DisplayName("نام و نام خانوادگی")]
        public string FullName => $"{FirstName} {LastName}";
        #endregion
    }
}
