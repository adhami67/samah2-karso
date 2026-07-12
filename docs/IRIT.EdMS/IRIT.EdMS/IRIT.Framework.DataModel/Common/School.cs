using IRIT.Framework.Common.Enum;
using IRIT.Framework.DataModel.License;
using IRIT.Framework.DataModel.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRIT.Framework.DataModel.Common
{
    [Table("School", Schema = "Common")]
    public class School
    {
        #region Constructor
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public School()
        {
            //BinaryLargeObjects = new HashSet<BinaryLargeObject>();
            //LookupKinds = new HashSet<LookupKind>();
        }
        #endregion

        #region  Data Columns
        [Column("Id", TypeName = "bigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }


        [Column("RenterId", TypeName = "bigint")]
        //[ForeignKey("FK_School_RenterId_2_Renter")]
        [ForeignKey("Renter")]
        public long? RenterId { get; set; }

        [Column("InfraStructureRingId", TypeName = "bigint")]
        //[ForeignKey("FK_School_InfraStructureRingId_2_InfraStructureRing")]
        [ForeignKey("InfraStructureRing")]
        public long InfraStructureRingId { get; set; }


        [Column("SchoolsGroupId", TypeName = "bigint")]
        //[ForeignKey("FK_School_SchoolsGroupId_2_SchoolGroup")]
        [ForeignKey("SchoolsGroup")]
        public long? SchoolsGroupId { get; set; }

        [Column("GenderType", TypeName = "int")]
        public GenderTypesEnum GenderType { get; set; }

        [Column("StudyGradeId", TypeName = "bigint")]
        //[ForeignKey("FK_School_GradeId_2_StudyGrade")]
        [ForeignKey("StudyGrade")]
        public long StudyGradeId { get; set; }

        [Column("Code", TypeName = "nvarchar(50)")]
        [StringLength(50)]
        public string Code { get; set; }

        [Column("SchoolName", TypeName = "nvarchar(255)")]
        [StringLength(255)]
        public string SchoolName { get; set; }

        [Column("IssueYear", TypeName = "int")]
        public int? IssueYear { get; set; }

        [Column("SmallImage", TypeName = "varbinary(max)")]
        public byte[] SmallImage { get; set; }

        [Column("PostAddress", TypeName = "nvarchar(max)")]
        public string PostAddress { get; set; }

        [Column("LocationId", TypeName = "bigint")]
        //[ForeignKey("FK_School_LocationId_Region")]
        [ForeignKey("Location")]
        public long? LocationId { get; set; }

        [Column("PhoneNo", TypeName = "nvarchar(max)")]
        public string PhoneNo { get; set; }

        [Column("MobileNo", TypeName = "nvarchar(max)")]
        public string MobileNo { get; set; }

        [Column("FaxNo", TypeName = "nvarchar(max)")]
        public string FaxNo { get; set; }

        [Column("Email", TypeName = "nvarchar(max)")]
        public string Email { get; set; }

        [Column("HomePageUrl", TypeName = "nvarchar(max)")]
        public string HomePageUrl { get; set; }

        [Column("Comment", TypeName = "nvarchar(max)")]
        public string Comment { get; set; }

        #region Common Data Columns
        [Column("IsDeleted", TypeName = "bit")]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Column("CreatorId", TypeName = "bigint")]
        //[ForeignKey("FK_School_CreatorId_2_Users")]
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

        #region Objects that depend on [School]

        #endregion

        #region Objects on witch [School] depends (ForeinKey Relationship)
        public virtual Renter Renter { get; set; }
        public virtual InfraStructureRing InfraStructureRing { get; set; }
        public virtual StudyGrade StudyGrade { get; set; }
        public virtual SchoolsGroup SchoolsGroup { get; set; }
        public virtual Region Location { get; set; }
        //public virtual User CreatorUser { get; set; }
        #endregion
    }
}
