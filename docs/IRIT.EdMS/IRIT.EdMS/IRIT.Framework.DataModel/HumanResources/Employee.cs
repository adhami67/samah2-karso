using IRIT.Framework.Common.Enum;
using IRIT.Framework.DataModel.Common;
using IRIT.Framework.DataModel.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRIT.Framework.DataModel.HumanResources
{
    [Table("Employee",Schema = "HumanResources")]
    public class Employee
    {
        #region Constructor
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            EmployeeEducationHistories = new HashSet<EmployeeEducationHistory>();
            EmployeeFamilyMembers = new HashSet<EmployeeFamilyMembers>();
            EmployeeJobHistories = new HashSet<EmployeeJobHistory>();
            EmployeeStatutes = new HashSet<EmployeeStatute>();
        }
        #endregion

        #region Data Columns
        [Column("Id", TypeName = "bigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Column("PartyId", TypeName = "bigint")]
        //[ForeignKey("FK_Employee_PartyId_2_Party")]
        [ForeignKey("Party")]
        public long PartyId { get; set; }

        [Column("EmploymentNumber", TypeName = "nvarchar(100)")]
        [Required]
        [StringLength(100)]
        public string EmploymentNumber { get; set; }

        [Column("EmployementType", TypeName = "int")]
        public EmploymentTypesEnum EmployementType { get; set; }

        [Column("MilitaryServiceType", TypeName = "int")]
        public MilitaryServiceTypesEnum MilitaryServiceType { get; set; }

        [Column("MilitaryServiceStartDate", TypeName = "DateTime")]
        public DateTime? MilitaryServiceStartDate { get; set; }
        [Column("MilitaryServiceFinishDate", TypeName = "DateTime")]
        public DateTime? MilitaryServiceFinishDate { get; set; }

        [Column("MarriageStatusType", TypeName = "int")]
        public MarriageStatusTypesEnum MarriageStatusType { get; set; }

        [Column("Comment", TypeName = "nvarchar(max)")]
        public string Comment { get; set; }

        #region Common Data Columns
        [Column("IsDeleted", TypeName = "bit")]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Column("CreatorId", TypeName = "bigint")]
        //[ForeignKey("FK_Employee_CreatorId_2_Users")]
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

        #region Objects that depend on [Employee]

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<EmployeeEducationHistory> EmployeeEducationHistories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<EmployeeFamilyMembers> EmployeeFamilyMembers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<EmployeeJobHistory> EmployeeJobHistories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<EmployeeStatute> EmployeeStatutes { get; set; }
        #endregion

        #region Objects on witch [Employee] depends (ForeinKey Relationship)
        public virtual Party Party { get; set; }


        public virtual User CreatorUser { get; set; }
        #endregion
    }
}
