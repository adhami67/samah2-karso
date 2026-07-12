using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRIT.Framework.DataModel.Workflow
{
    [Table("Workflow.WorkMessages")]
    public sealed class WorkMessage
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WorkMessage()
        {
            WorkMessages1 = new HashSet<WorkMessage>();
            WorkMessageSeenHistories = new HashSet<WorkMessageSeenHistory>();
        }

        public long Id { get; set; }

        public long WorkId { get; set; }

        public long? WorkMessageParentId { get; set; }

        public long SenderStatuteId { get; set; }

        public long SenderUserId { get; set; }

        [Required]
        [StringLength(255)]
        public string WorkMessageSubject { get; set; }

        public DateTime? SendTime { get; set; }

        public string Comment { get; set; }

        public bool IsDeleted { get; set; }

        public long? CreatorId { get; set; }

        public DateTime CreateTime { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] TimeTag { get; set; }

        public Work Work { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<WorkMessage> WorkMessages1 { get; set; }

        public WorkMessage WorkMessage1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<WorkMessageSeenHistory> WorkMessageSeenHistories { get; set; }
    }
}
