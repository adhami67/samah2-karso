using IRIT.Framework.Common.Enum;
using IRIT.Framework.DataModel.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRIT.Framework.DataModel.Common
{
    [Table("TableExtensionProperties", Schema = "Common")]
    public class TableExtensionProperties
    {

        #region Constructor
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TableExtensionProperties()
        {
            TableExtensionValues = new HashSet<TableExtensionValues>();
        }

        #endregion

        #region Data Columns
        [Column("Id", TypeName = "bigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Column("TableExtensionId", TypeName = "bigint")]
        //[ForeignKey("FK_TableExtensionProperties_TableExtensionId_2_TableExtensions")]
        [ForeignKey("TableExtensions")]
        public long TableExtensionId { get; set; }

        [Column("TagShow", TypeName = "bit")]
        public bool TagShow { get; set; }

        [Column("TagCaption", TypeName = "nvarchar(max)")]
        public string TagCaption { get; set; }

        [Column("TagDataType", TypeName = "int")]
        public DataTypes TagDataType { get; set; }

        [Column("TagRequire", TypeName = "bit")]
        [DefaultValue("false")]
        public bool TagRequire { get; set; }

        [Column("Comment", TypeName = "nvarchar(max)")]
        public string Comment { get; set; }

        #region Common Data Columns
        [Column("IsDeleted", TypeName = "bit")]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Column("CreatorId", TypeName = "bigint")]
        //[ForeignKey("FK_TableExtensionProperties_CreatorId_2_Users")]
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

        #region Objects that depend on [TableExtensionProperties]

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<TableExtensionValues> TableExtensionValues { get; set; }


        #endregion

        #region Objects on witch [TableExtensionProperties] depends (ForeinKey Relationship)
        public virtual TableExtensions TableExtensions { get; set; }
        public virtual User CreatorUser { get; set; }
        #endregion
    }
}
