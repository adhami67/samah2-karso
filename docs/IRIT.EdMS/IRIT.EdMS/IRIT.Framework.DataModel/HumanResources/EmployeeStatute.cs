using IRIT.Framework.Common.Enum;
using IRIT.Framework.DataModel.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRIT.Framework.DataModel.HumanResources
{
    [Table("EmployeeStatute",Schema = "HumanResources")]
    public class EmployeeStatute
    {
        #region Constructor
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EmployeeStatute()
        {
          
        }
        #endregion

        #region Data Column

        [Column("Id", TypeName = "bigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Column("EmployeeId", TypeName = "bigint")]
        //[ForeignKey("FK_EmployeeStatute_EmployeeId_2_Employee")]
        [ForeignKey("Employee")]
        public long EmployeeId { get; set; }

        [Column("OrganizationStructureId", TypeName = "bigint")]
        //[ForeignKey("FK_EmployeeStatute_OrganizationStructureId_2_OrganizationStructure")]
        [ForeignKey("OrganizationStructure")]
        public long OrganizationStructureId { get; set; }

        [Column("OrganizationRoleId", TypeName = "bigint")]
        //[ForeignKey("FK_EmployeeStatute_OrganizationRoleId_2_OrganizationStructure")]
        [ForeignKey("OrganizationRole")]
        public long OrganizationRoleId { get; set; }

        [Column("StatuteType", TypeName = "int")]
        public StatuteTypesEnum StatuteType { get; set; }

        [Column("IssueDate", TypeName = "DateTime")]
        public DateTime IssueDate { get; set; }

        [Column("ApplyDate", TypeName = "DateTime")]
        public DateTime ApplyDate { get; set; }

        [Column("ExpiryDate", TypeName = "DateTime")]
        public DateTime? ExpiryDate { get; set; }

        [Column("Comment", TypeName = "nvarchar(max)")]
        public string Comment { get; set; }

        #region Common Data Columns
        [Column("IsDeleted", TypeName = "bit")]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Column("CreatorId", TypeName = "bigint")]
        //[ForeignKey("FK_EmployeeStatute_CreatorId_2_Users")]
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

        #region Objects that depend on [EmployeeStatute]

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<EmployeeEducationHistory> EmployeeEducationHistories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<EmployeeFamilyMembers> EmployeeFamilyMembers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<EmployeeJobHistory> EmployeeJobHistories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<EmployeeStatute> EmployeeStatutes { get; set; }
        #endregion

        #region Objects on witch [EmployeeStatute] depends (ForeinKey Relationship)
        public virtual Employee Employee { get; set; }
        public virtual OrganizationStructure OrganizationStructure { get; set; }
        public virtual OrganizationStructure OrganizationRole { get; set; }
        public virtual User CreatorUser { get; set; }
        #endregion
    }
}
