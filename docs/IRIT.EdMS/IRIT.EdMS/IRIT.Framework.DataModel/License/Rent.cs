using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IRIT.Framework.DataModel.Security;

namespace IRIT.Framework.DataModel.License
{
    [Table("License.Rent")]
    public  class Rent
    {
        #region Constructor

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Rent()
        {

        }
        #endregion

        #region  Data Columns
        [Column("Id", TypeName = "bigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Column("RentType", TypeName = "int")]
        public int RentType { get; set; }

        [Column("Code", TypeName = "nvarchar(50)")]
        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [Column("Title", TypeName = "nvarchar(max)")]
        [Required]
        public string Title { get; set; }

        [Column("Comment", TypeName = "nvarchar(max)")]
        public string Comment { get; set; }

        #region Common Data Columns
        [Column("IsDeleted", TypeName = "bit")]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Column("CreatorId", TypeName = "bigint")]
        //[ForeignKey("FK_Rent_CreatorId_2_Users")]
        public long CreatorId { get; set; }

        private DateTime? _createTime;
        [Column("CreateTime", TypeName = "DateTime")]
        public DateTime CreateTime
        {
            get
            {
                return _createTime ?? DateTime.Now;
            }

            private set { _createTime = value; }
        }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] TimeTag { get; set; }

        #endregion
        #endregion

        #region Objects that depend on [Rent]

        #endregion

        #region Objects on witch [Rent] depends (ForeinKey Relationship)

        public virtual User CreatorUser { get; set; }
        #endregion
    }
}
