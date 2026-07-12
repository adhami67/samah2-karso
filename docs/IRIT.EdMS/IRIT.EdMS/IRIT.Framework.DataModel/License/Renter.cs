using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IRIT.Framework.DataModel.Security;

namespace IRIT.Framework.DataModel.License
{
    [Table("Renter",Schema = "License")]
    public class Renter
    {
        #region Constructor

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Renter()
        {
           
        }
        #endregion

        #region Data Columns
        [Column("Id", TypeName = "bigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Column("RenterTitle", TypeName = "nvarchar(100)")]
        [StringLength(100)]
        public string RenterTitle { get; set; }

        [Column("RenterName", TypeName = "nvarchar(max)")]
        public string RenterName { get; set; }

        [Column("NationalCode", TypeName = "nvarchar(10)")]
        [StringLength(10)]
        public string NationalCode { get; set; }

        [Column("EconomicCode", TypeName = "nvarchar(100)")]
        [StringLength(100)]
        public string EconomicCode { get; set; }

        [Column("RegistrationNo", TypeName = "nvarchar(100)")]
        [StringLength(100)]
        public string RegistrationNo { get; set; }

        [Column("PostAddress", TypeName = "nvarchar(max)")]
        public string PostAddress { get; set; }

        [Column("PhoneNo", TypeName = "nvarchar(max)")]
        public string PhoneNo { get; set; }

        [Column("MobileNo", TypeName = "nvarchar(max)")]
        public string MobileNo { get; set; }

        [Column("FaxNo", TypeName = "nvarchar(max)")]
        public string FaxNo { get; set; }

        [Column("Email", TypeName = "nvarchar(max)")]
        public string Email { get; set; }

        [Column("HomePageURL", TypeName = "nvarchar(max)")]
        public string HomePageURL { get; set; }

        [Column("Comment", TypeName = "nvarchar(max)")]
        public string Comment { get; set; }

        #region Common Data Columns
        [Column("IsDeleted", TypeName = "bit")]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Column("CreatorId", TypeName = "bigint")]
        //[ForeignKey("FK_Renter_CreatorId_2_Users")]
        public long CreatorId { get; set; }

        private DateTime? createTime;
        [Column("CreateTime", TypeName = "DateTime")]
        public DateTime CreateTime
        {
            get
            {
                return createTime ?? DateTime.Now;
            }

            private set { createTime = value; }
        }

        [Column("TimeTag", TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] TimeTag { get; set; }

        #endregion
        #endregion

        #region Objects that depend on [Renter]

        #endregion

        #region Objects on witch [Renter] depends (ForeinKey Relationship)
       
        public virtual User CreatorUser { get; set; }
        #endregion
    }
}
