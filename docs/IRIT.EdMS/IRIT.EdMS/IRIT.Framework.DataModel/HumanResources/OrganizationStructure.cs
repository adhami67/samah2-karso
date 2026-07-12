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
    [Table("OrganizationStructure", Schema = "HumanResources")]
    public class OrganizationStructure
    {
        #region Constructor

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
            "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrganizationStructure()
        {
            EmployeeStatutes = new HashSet<EmployeeStatute>();
            // OrganizationStructure = new HashSet<OrganizationStructure>();
        }

        #endregion

        #region Data Column

        [Column("Id", TypeName = "bigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Column("SchoolId", TypeName = "bigint")]
        //[ForeignKey("FK_OrganizationStructure_SchoolId_2_School")]
        [ForeignKey("School")]
        public long SchoolId { get; set; }

        [Column("StructureType", TypeName = "int")]
        public StructureTypesEnum StructureType { get; set; }


        [Column("ParentId", TypeName = "bigint")]
        //[ForeignKey("FK_OrganizationStructure_ParentId_2_OrganizationStructure")]
        [ForeignKey("Parent")]
        public long? ParentId { get; set; }


        [Column("Code", TypeName = "nvarchar(50)")]
        [StringLength(50)]
        public string Code { get; set; }

        [Column("Title", TypeName = "nvarchar(255)")]
        [Required]
        [StringLength(255)]
        public string Title { get; set; }


        [Column("Capacity", TypeName = "int")]
        [DefaultValue(0)]
        public int Capacity { get; set; }


        [Column("ActivationDate", TypeName = "DateTime")]
        public DateTime ActivationDate { get; set; }

        [Column("ClosingDate", TypeName = "DateTime")]
        public DateTime? ClosingDate { get; set; }

        [Column("Comment", TypeName = "nvarchar(max)")]
        public string Comment { get; set; }

        #region Common Data Columns

        [Column("IsDeleted", TypeName = "bit")]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Column("CreatorId", TypeName = "bigint")]
        //[ForeignKey("FK_OrganizationStructure_CreatorId_2_Users")]
        public long CreatorId { get; set; }

        private DateTime? createTime;

        [Column("CreateTime", TypeName = "DateTime")]
        public DateTime CreateTime
        {
            get { return createTime ?? DateTime.Now; }

            set { createTime = value; }
        }

        [Column("TimeTag", TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] TimeTag { get; set; }

        #endregion

        #endregion

        #region Objects that depend on [OrganizationStructure]

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
            "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<EmployeeEducationHistory> EmployeeEducationHistories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
            "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<EmployeeFamilyMembers> EmployeeFamilyMembers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
            "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<EmployeeJobHistory> EmployeeJobHistories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
            "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<EmployeeStatute> EmployeeStatutes { get; set; }

        #endregion

        #region Objects on witch [OrganizationStructure] depends (ForeinKey Relationship)

        public virtual School School { get; set; }

        public virtual OrganizationStructure Parent { get; set; }
        public virtual User CreatorUser { get; set; }

        #endregion
    }
}
