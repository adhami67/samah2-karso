using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRIT.Framework.DataModel.Security
{
    [Table("AccessibleResources",Schema = "Security")]
    public  class AccessibleResource
    {
        #region Constructor


        #endregion

        #region Data Columns
        [Column("Id", TypeName = "bigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Column("ParentId", TypeName = "bigint")]
        //[ForeignKey("FK_AccessibleResource_ParentId_2_AccessibleResource")]
        [ForeignKey("Parent")]
        public long? ParentId { get; set; }

        [Column("ResourceType", TypeName = "bigint")]
        public long ResourceType { get; set; }

        [Column("ResourceKey", TypeName = "nvarchar(max)")]
        [Required]
        public string ResourceKey { get; set; }

        [Column("ResourceName", TypeName = "nvarchar(max)")]
        [Required]
        public string ResourceName { get; set; }

        [Column("HaveRowAccess", TypeName = "bit")]
        [DefaultValue(false)]
        public bool HaveRowAccess { get; set; }

        #region Common Data Columns
        [Column("IsDeleted", TypeName = "bit")]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Column("CreatorId", TypeName = "bigint")]
        //[ForeignKey("CreatorUser")]
        public long CreatorId { get; set; }

        private DateTime? _createTime;
        [Column("CreateTime", TypeName = "DateTime")]
        public DateTime CreateTime
        {
            get
            {
                return _createTime ?? DateTime.Now;
            }

            set { _createTime = value; }
        }

        [Column("TimeTag", TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] TimeTag { get; set; }

        #endregion
        #endregion

        #region Objects that depend on [AccessibleResource]

        #endregion

        #region Objects on witch [AccessibleResource] depends (ForeinKey Relationship)
        public virtual AccessibleResource Parent { get; set; }
        public virtual User CreatorUser { get; set; }
        #endregion
    }
}
