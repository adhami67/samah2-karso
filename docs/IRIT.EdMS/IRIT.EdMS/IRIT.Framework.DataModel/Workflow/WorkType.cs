using IRIT.Framework.DataModel.HumanResources;
using IRIT.Framework.DataModel.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRIT.Framework.DataModel.Workflow
{
    [Table("WorkType",Schema = "Workflow")]
    public  class WorkType
    {
        #region Constructor
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WorkType()
        {
            Works = new HashSet<Work>();
            WorkReceivers = new HashSet<WorkReceiver>();
        }
        #endregion

        #region Data Column
        [Column("Id", TypeName = "bigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Column("WorkTypeCode", TypeName = "nvarchar(50)")]
        [Required]
        [StringLength(50)]
        public string WorkTypeCode { get; set; }

        [Column("WorkTypeTitle", TypeName = "nvarchar(255)")]
        [Required]
        [StringLength(255)]
        public string WorkTypeTitle { get; set; }

        [Column("WorkTypeColor", TypeName = "nvarchar(20)")]
        [Required]
        [StringLength(20)]
        public string WorkTypeColor { get; set; }

        [Column("WorkTypeIcon", TypeName = "varbinary(max)")]
        [Required]
        public byte[] WorkTypeIcon { get; set; }


        [Column("Comment", TypeName = "nvarchar(max)")]
        public string Comment { get; set; }

        #region Common Data Columns
        [Column("IsDeleted", TypeName = "bit")]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Column("CreatorId", TypeName = "bigint")]
        //[ForeignKey("FK_WorkType_CreatorId_2_Users")]
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

        #region Objects that depend on [WorkType]

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Work> Works { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<WorkReceiver> WorkReceivers { get; set; }
        #endregion

        #region Objects on witch [WorkType] depends (ForeinKey Relationship)

        public virtual User CreatorUser { get; set; }
        #endregion
    }
}
