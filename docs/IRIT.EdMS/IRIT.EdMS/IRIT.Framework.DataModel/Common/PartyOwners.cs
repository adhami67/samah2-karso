using IRIT.Framework.Common.Enum;
using IRIT.Framework.DataModel.Security;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRIT.Framework.DataModel.Common
{
    [Table("PartyOwners", Schema = "Common")]
    public class PartyOwners
    {
        #region Constructor


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PartyOwners()
        {

        }
        #endregion

        #region Data Columns
        [Column("Id", TypeName = "bigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Column("PartyId", TypeName = "bigint")]
        //[ForeignKey("FK_PartyOwners_PartyId_2_Party")]
        [ForeignKey("Party")]
        public long? PartyId { get; set; }

        [Column("OwnerType", TypeName = "int")]
        public OwnerTypesEnum OwnerType { get; set; }

        public long? OwnerId { get; set; }

        [Column("Comment", TypeName = "nvarchar(max)")]
        public string Comment { get; set; }

        #region Common Data Columns
        [Column("IsDeleted", TypeName = "bit")]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Column("CreatorId", TypeName = "bigint")]
        //[ForeignKey("FK_PartyOwners_CreatorId_2_Users")]
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

        #region Objects that depend on [PartyOwners]




        #endregion

        #region Objects on witch [PartyOwners] depends (ForeinKey Relationship)
        public Party Party { get; set; }

        public virtual User CreatorUser { get; set; }
        #endregion
    }
}
