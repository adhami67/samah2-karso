using IRIT.Framework.DataModel.Security;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRIT.Framework.DataModel.Common
{
    [Table("TableExtensionValues", Schema = "Common")]
    public class TableExtensionValues
    {
        #region Constructor
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TableExtensionValues()
        {

        }

        #endregion

        #region Data Columns
        [Column("Id", TypeName = "bigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Column("TableExtensionPropertyId", TypeName = "bigint")]
        //[ForeignKey("FK_TableExtensionValues_TableExtensionPropertyId_2_TableExtensionProperties")]
        [ForeignKey("TableExtensionProperties")]
        public long TableExtensionPropertyId { get; set; }

        [Column("TagValue", TypeName = "nvarchar(max)")]
        [Required]
        public string TagValue { get; set; }

        #region Common Data Columns
        [Column("IsDeleted", TypeName = "bit")]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Column("CreatorId", TypeName = "bigint")]
        //[ForeignKey("FK_TableExtensionValues_CreatorId_2_Users")]
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

        #region Objects on witch [TableExtensionProperties] depends (ForeinKey Relationship)
        public virtual TableExtensionProperties TableExtensionProperties { get; set; }
        public virtual User CreatorUser { get; set; }
        #endregion
    }
}
