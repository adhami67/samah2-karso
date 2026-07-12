using IRIT.Framework.Common.Enum;
using IRIT.Framework.DataModel.Security;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IRIT.Framework.DataModel.Common;

namespace IRIT.Framework.DataModel.HumanResources
{
    [Table("EmployeeFamilyMembers",Schema = "HumanResources")]
    public class EmployeeFamilyMembers
    {
        #region Constructor
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EmployeeFamilyMembers()
        {

        }


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

        [Column("FamilyMemberType", TypeName = "int")]
        public FamilyMemberTypes FamilyMemberType { get; set; }

        [Column("PartyId", TypeName = "int")]
        //[ForeignKey("FK_EmployeeFamilyMembers_PartyId_2_Party")]
        [ForeignKey("Party")]
        public long? PartyId { get; set; }

        [Column("FirstName", TypeName = "nvarchar(50)")]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Column("LastName", TypeName = "nvarchar(155)")]
        [StringLength(155)]
        public string LastName { get; set; }

        [Column("NationalCode", TypeName = "nvarchar(10)")]
        [StringLength(10)]
        public string NationalCode { get; set; }

        [Column("GenderType", TypeName = "int")]
        public GenderTypes GenderType { get; set; }

        [Column("BirthDate", TypeName = "DateTime")]
        public DateTime? BirthDate { get; set; }

        [Column("Comment", TypeName = "nvarchar(max)")]
        public string Comment { get; set; }

        #region Common Data Columns
        [Column("IsDeleted", TypeName = "bit")]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Column("CreatorId", TypeName = "bigint")]
        //[ForeignKey("FK_EmployeeFamilyMembers_CreatorId_2_Users")]
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

        #region Objects that depend on [EmployeeFamilyMembers]


        #endregion

        #region Objects on witch [EmployeeFamilyMembers] depends (ForeinKey Relationship)
        public virtual Employee Employee { get; set; }
        public virtual Party Party { get; set; }
        public virtual User CreatorUser { get; set; }
        #endregion
    }
}
