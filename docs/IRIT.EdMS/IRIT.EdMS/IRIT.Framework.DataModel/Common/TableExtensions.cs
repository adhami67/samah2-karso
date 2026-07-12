using IRIT.Framework.DataModel.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRIT.Framework.DataModel.Common
{
    [Table("TableExtensions", Schema = "Common")]
    public class TableExtensions
    {

        #region Constructor
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TableExtensions()
        {
            TableExtensionProperties =new HashSet<TableExtensionProperties>();
        }

        #endregion

        #region Data Columns
        [Column("Id", TypeName = "bigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Column("InfraStructureRingId", TypeName = "bigint")]
        //[ForeignKey("FK_TableExtensions_InfraStructureRingId_2_InfraStructureRing")]
        [ForeignKey("InfraStructureRing")]
        public long InfraStructureRingId { get; set; }

        [Column("TableName", TypeName = "nvarchar(max)")]
        [Required]
        public string TableName { get; set; }

        [Column("TagName", TypeName = "nvarchar(max)")]
        [Required]
        public string TagName { get; set; }

        [Column("Comment", TypeName = "nvarchar(max)")]
        public string Comment { get; set; }

        #region Common Data Columns
        [Column("IsDeleted", TypeName = "bit")]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Column("CreatorId", TypeName = "bigint")]
        //[ForeignKey("FK_TableExtentions_CreatorId_2_Users")]
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

        #region Objects that depend on [TableExtensions]

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<TableExtensionProperties> TableExtensionProperties { get; set; }

        #endregion

        #region Objects on witch [TableExtensions] depends (ForeinKey Relationship)
        public virtual InfraStructureRing InfraStructureRing { get; set; }
        public virtual User CreatorUser { get; set; }
        #endregion
    }
}
