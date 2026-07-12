using IRIT.Framework.DataModel.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IRIT.Framework.Common.Enum;

namespace IRIT.Framework.DataModel.Common
{
    [Table("Party", Schema = "Common")]
    public class Party
    {
        #region Constructor


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Party()
        {
            PartyAddresses = new HashSet<PartyAddress>();
            PartyOwners = new HashSet<PartyOwners>();
        }
        #endregion

        #region Data Columns
        [Column("Id", TypeName = "bigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Column("PartyType", TypeName = "int")]
        public PartyTypesEnum PartyType { get; set; }

        #region Person Info
        [Column("FirstName", TypeName = "nvarchar(100)")]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Column("LastName", TypeName = "nvarchar(155)")]
        [StringLength(155)]
        public string LastName { get; set; }

        [Column("NickName", TypeName = "nvarchar(100)")]
        [StringLength(100)]
        public string NickName { get; set; }

        [Column("NationalCode", TypeName = "nvarchar(10)")]
        [StringLength(10)]
        public string NationalCode { get; set; }

        [Column("NationalityId", TypeName = "bigint")]
        //[ForeignKey("FK_Party_NationalityId_2_Region")]
        [ForeignKey("Nationality")]
        public long? NationalityId { get; set; }


        [Column("AllegianceId", TypeName = "bigint")]
        //[ForeignKey("FK_Party_AllegianceId_2_Region")]
        [ForeignKey("Allegiance")]
        public long? AllegianceId { get; set; }

        [Column("ReligionId", TypeName = "bigint")]
        //[ForeignKey("FK_Party_ReligionId_2_Religion")]
        [ForeignKey("Religion")]
        public long? ReligionId { get; set; }

        [Column("SubReligionId", TypeName = "bigint")]
        //[ForeignKey("FK_Party_SubReligionId_2_Religion")]
        [ForeignKey("SubReligion")]
        public long? SubReligionId { get; set; }

        [Column("IdNumber", TypeName = "nvarchar(20)")]
        [StringLength(20)]
        public string IdNumber { get; set; }

        [Column("IdSerial", TypeName = "nvarchar(20)")]
        [StringLength(20)]
        public string IdSerial { get; set; }

        [Column("GenderType", TypeName = "int")]
        public GenderTypesEnum GenderType { get; set; }

        [Column("FatherName", TypeName = "nvarchar(100)")]
        [StringLength(100)]
        public string FatherName { get; set; }

        [Column("BirthDate", TypeName = "DateTime")]
        public DateTime? BirthDate { get; set; }

        [Column("BirthPlaceId", TypeName = "bigint")]
        //[ForeignKey("FK_Party_BirthPlaceId_2_Region")]
        [ForeignKey("BirthPlace")]
        public long? BirthPlaceId { get; set; }

        [Column("IssueDate", TypeName = "DateTime")]
        public DateTime? IssueDate { get; set; }


        [Column("IssuancePlaceId", TypeName = "bigint")]
        //[ForeignKey("FK_Party_IssuancePlaceId_2_Region")]
        [ForeignKey("IssuancePlace")]
        public long? IssuancePlaceId { get; set; }
        #endregion

        #region Legal Man Info(Company)
        [Column("CompanyName", TypeName = "nvarchar(255)")]
        [StringLength(255)]
        public string CompanyName { get; set; }

        [Column("EconomicCode", TypeName = "nvarchar(100)")]
        [StringLength(100)]
        public string EconomicCode { get; set; }

        [Column("RegistrationNo", TypeName = "nvarchar(100)")]
        [StringLength(100)]
        public string RegistrationNo { get; set; }

        [Column("CompanyKindId", TypeName = "bigint")]
        //[ForeignKey("FK_Party_CompanyKindId_2_Lookup")]
        [ForeignKey("CompanyKind")]
        public long? CompanyKindId { get; set; }

        [Column("CompanyActivityKindId", TypeName = "bigint")]
        //[ForeignKey("FK_Party_CompanyActivityKindId_2_Lookup")]
        [ForeignKey("CompanyActivityKind")]
        public long? CompanyActivityKindId { get; set; }

        #endregion

        [Column("SmallImage", TypeName = "varbinary(max)")]
        public byte?[] SmallImage { get; set; }

        [Column("ImageId", TypeName = "bigint")]
        //[ForeignKey("FK_Party_ImageId_Blob")]
        [ForeignKey("Image")]
        public long? ImageId { get; set; }

        [Column("SignatureImageId", TypeName = "bigint")]
        //[ForeignKey("FK_Party_SignatureImageId_Blob")]
        [ForeignKey("SignatureImage")]
        public long? SignatureImageId { get; set; }


        [Column("PostAddress", TypeName = "nvarchar(max)")]
        public string PostAddress { get; set; }

        [Column("LocationId", TypeName = "bigint")]
        //[ForeignKey("FK_Party_LocationId_Region")]
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

        public string HomePageUrl { get; set; }

        [Column("Comment", TypeName = "nvarchar(max)")]
        public string Comment { get; set; }

        #region Common Data Columns
        [Column("IsDeleted", TypeName = "bit")]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Column("CreatorId", TypeName = "bigint")]
        //[ForeignKey("FK_Party_CreatorId_2_Users")]
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

        #region Objects that depend on [Party]


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<PartyAddress> PartyAddresses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<PartyOwners> PartyOwners { get; set; }


        #endregion

        #region Objects on witch [Party] depends (ForeinKey Relationship)
        public virtual BinaryLargeObject Image { get; set; }

        public virtual BinaryLargeObject SignatureImage { get; set; }

        public virtual Lookup CompanyActivityKind { get; set; }

        public virtual Lookup CompanyKind { get; set; }

        public virtual Region Location { get; set; }

        public virtual Region IssuancePlace { get; set; }

        public virtual Region BirthPlace { get; set; }

        public virtual Region Nationality { get; set; }

        public virtual Region Allegiance { get; set; }

        public virtual Religion Religion { get; set; }

        public virtual Religion SubReligion { get; set; }

        //public virtual User User { get; set; }
        #endregion
    }
}
