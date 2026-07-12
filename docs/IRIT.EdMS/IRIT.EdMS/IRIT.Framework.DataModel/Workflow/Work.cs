using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRIT.Framework.DataModel.Workflow
{
    [Table("Work", Schema = "Workflow")]
    public  class Work
    {

        #region Constructor
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Work()
        {
            Work1 = new HashSet<Work>();
            WorkAttachments = new HashSet<WorkAttachment>();
            WorkEvaluationHistories = new HashSet<WorkEvaluationHistory>();
            WorkMessages = new HashSet<WorkMessage>();
            WorkPermissions = new HashSet<WorkPermission>();
            WorkReceivers = new HashSet<WorkReceiver>();
            WorkStatusChangeHistories = new HashSet<WorkStatusChangeHistory>();
        }
        #endregion


        #region Data Column
        [Column("Id", TypeName = "bigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [ForeignKey("FK_Work_WorkGroupId_2_WorkGroup")]
        [Column("WorkGroupId", TypeName = "bigint")]
        public long WorkGroupId { get; set; }

        [ForeignKey("FK_Work_WorkTypeId_2_WorkType")]
        [Column("WorkTypeId", TypeName = "bigint")]
        public long WorkTypeId { get; set; }

        [ForeignKey("FK_Work_WorkPriorityId_2_WorkPriority")]
        [Column("WorkPriorityId", TypeName = "bigint")]
        public long WorkPriorityId { get; set; }

        [ForeignKey("FK_Work_WorkParentId_2_Work")]
        [Column("WorkParentId", TypeName = "bigint")]
        public long? WorkParentId { get; set; }

        [ForeignKey("FK_Work_SenderStatuteId_2_EmployeeStatute")]
        [Column("SenderStatuteId", TypeName = "bigint")]
        public long SenderStatuteId { get; set; }
        [ForeignKey("FK_Work_SenderUserId_2_Users")]
        [Column("SenderUserId", TypeName = "bigint")]
        public long SenderUserId { get; set; }

        [Column("WorkSubject", TypeName = "nvarchar(255)")]
        [Required]
        [StringLength(255)]
        public string WorkSubject { get; set; }

        [Column("SendTime", TypeName = "DateTime")]
        public DateTime? SendTime { get; set; }

        [Column("Comment", TypeName = "nvarchar(max)")]
        public string Comment { get; set; }

        #region Common Data Columns
        [Column("IsDeleted", TypeName = "bit")]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Column("CreatorId", TypeName = "bigint")]
        [ForeignKey("FK_Work_CreatorId_2_Users")]
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

        #region Objects that depend on [Work]

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Work> Works { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<WorkReceiver> WorkReceivers { get; set; }
        #endregion

        #region Objects on witch [WorkType] depends (ForeinKey Relationship)

        public virtual User CreatorUser { get; set; }
        #endregion

        public WorkGroup WorkGroup { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Work> Works { get; set; }

        public Work Parent { get; set; }

        public WorkPriority WorkPriority { get; set; }

        public WorkType WorkType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<WorkAttachment> WorkAttachments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<WorkEvaluationHistory> WorkEvaluationHistories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<WorkMessage> WorkMessages { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<WorkPermission> WorkPermissions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<WorkReceiver> WorkReceivers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<WorkStatusChangeHistory> WorkStatusChangeHistories { get; set; }
    }
}
