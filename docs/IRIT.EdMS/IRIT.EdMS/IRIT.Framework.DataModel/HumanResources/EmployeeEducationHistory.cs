using IRIT.Framework.DataModel.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IRIT.Framework.DataModel.Common;

namespace IRIT.Framework.DataModel.HumanResources
{
    [Table("EmployeeEducationHistory",Schema = "HumanResources")]
    public class EmployeeEducationHistory
    {
        #region Constructor
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EmployeeEducationHistory()
        {

        }

        #endregion

        #region Data Column
        [Column("Id", TypeName = "bigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }


        [Column("EmployeeId", TypeName = "bigint")]
        //[ForeignKey("FK_EmployeeEducationHistory_EmployeeId_2_Employee")]
        [ForeignKey("Employee")]
        public long EmployeeId { get; set; }

       
        [Column("StudyGradeId", TypeName = "bigint")]
        //[ForeignKey("FK_EmployeeEducationHistory_StudyGradeId_2_StudyGrade")]
        [ForeignKey("StudyGrade")]
        public long StudyGradeId { get; set; }

        [Column("CourseOfStudy", TypeName = "nvarchar(max)")]
        public string CourseOfStudy { get; set; }

        [Column("FromDate", TypeName = "DateTime")]
        public DateTime FromDate { get; set; }

        [Column("ToDate", TypeName = "DateTime")]
        public DateTime ToDate { get; set; }

        [Column("EducationCenterName", TypeName = "nvarchar(max)")]
        [Required]
        public string EducationCenterName { get; set; }

        [Column("Quality", TypeName = "float")]
        [Required]
        public Double Quality { get; set; }

        [Column("EffectiveDate", TypeName = "DateTime")]
        public DateTime EffectiveDate { get; set; }

        [Column("Comment", TypeName = "nvarchar(max)")]
        public string Comment { get; set; }

        #region Common Data Columns
        [Column("IsDeleted", TypeName = "bit")]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Column("CreatorId", TypeName = "bigint")]
        //[ForeignKey("FK_EmployeeEducationHistory_CreatorId_2_Users")]
        public long CreatorId { get; set; }

        private DateTime? createTime;
        [Column("CreateTime", TypeName = "DateTime")]
        public DateTime CreateTime
        {
            get
            {
                return createTime ?? DateTime.Now;
            }

            set { createTime = value; }
        }

        [Column("TimeTag", TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] TimeTag { get; set; }
        #endregion
        #endregion

        #region Objects that depend on [EmployeeEducationHistory]

       
        #endregion

        #region Objects on witch [EmployeeEducationHistory] depends (ForeinKey Relationship)
        public virtual Employee Employee { get; set; }

        public virtual StudyGrade StudyGrade { get; set; }

        public virtual User CreatorUser { get; set; }
        #endregion
    }
}
