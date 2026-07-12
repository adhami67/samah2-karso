using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IRIT.Framework.DataModel.Security;
using IRIT.Framework.DataModel.License;

namespace IRIT.Framework.DataModel.Common
{
    [Table("Department",Schema = "Common")]
    public class Department
    {
        #region Constructor
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Department()
        {
            
        }
        #endregion

        #region Data Columns


        [Column("Id", TypeName = "bigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }


        [Column("RenterId", TypeName = "bigint")]
        //[ForeignKey("FK_Department_RenterId_2_Renter")]
        [ForeignKey("Renter")]
        public long? RenterId { get; set; }

        [Column("InfraStructureRingId", TypeName = "bigint")]
        //[ForeignKey("FK_Department_InfraStructureRingId_2_InfraStructureRing")]
        [ForeignKey("InfraStructureRing")]
        public long InfraStructureRingId { get; set; }

        [Column("Code", TypeName = "nvarchar(50)")]
        [StringLength(50)]
        public string Code { get; set; }

        [Column("DepartmentName", TypeName = "nvarchar(255)")]
        [StringLength(255)]
        public string DepartmentName { get; set; }

        [Column("IssueDate", TypeName = "DateTime")]
        public DateTime? IssueDate { get; set; }

        [Column("SmallImage", TypeName = "varbinary(max)")]
        public byte?[] SmallImage { get; set; }

        [Column("PostAddress", TypeName = "nvarchar(max)")]
        public string PostAddress { get; set; }

        [Column("LocationId", TypeName = "bigint")]
        //[ForeignKey("FK_Department_LocationId_Region")]
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
        //[ForeignKey("FK_Department_CreatorId_2_Users")]
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

        #region Objects that depend on [Department]
       
        #endregion

        #region Objects on witch [Department] depends (ForeinKey Relationship)
        public virtual Renter Renter { get; set; }
        public virtual InfraStructureRing InfraStructureRing { get; set; }
        public virtual Region Location { get; set; }
        public virtual User CreatorUser { get; set; }
        #endregion
    }
}
