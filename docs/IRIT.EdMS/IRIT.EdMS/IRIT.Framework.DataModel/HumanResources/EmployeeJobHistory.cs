using IRIT.Framework.DataModel.Security;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRIT.Framework.DataModel.HumanResources
{
    [Table("EmployeeJobHistory",Schema = "HumanResources")]
    public class EmployeeJobHistory
    {
        #region Constructor
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EmployeeJobHistory()
        { }
        #endregion

        #region Data Column
        [Column("Id", TypeName = "bigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Column("EmployeeId", TypeName = "bigint")]
        //[ForeignKey("FK_EmployeeFamilyMembers_EmployeeId_2_Employee")]
        [ForeignKey("Employee")]
        public long EmployeeId { get; set; }


        [Column("JobTitle", TypeName = "nvarchar(max)")]
        [StringLength(100)]
        public string JobTitle { get; set; }

        [Column("FromDate", TypeName = "DateTime")]
        public DateTime FromDate { get; set; }

        [Column("ToDate", TypeName = "DateTime")]
        public DateTime ToDate { get; set; }

        [Column("WorkCenterName", TypeName = "nvarchar(max)")]
        [Required]
        public string WorkCenterName { get; set; }

        [Column("Comment", TypeName = "nvarchar(max)")]
        public string Comment { get; set; }

        #region Common Data Columns
        [Column("IsDeleted", TypeName = "bit")]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Column("CreatorId", TypeName = "bigint")]
        //[ForeignKey("FK_EmployeeJobHistory_CreatorId_2_Users")]
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

        #region Objects that depend on [EmployeeJobHistory]


        #endregion

        #region Objects on witch [EmployeeJobHistory] depends (ForeinKey Relationship)
        public virtual Employee Employee { get; set; }
        public virtual User CreatorUser { get; set; }
        #endregion
    }
}
