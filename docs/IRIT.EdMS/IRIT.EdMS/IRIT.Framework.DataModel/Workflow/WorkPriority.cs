using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRIT.Framework.DataModel.Workflow
{
    [Table("Workflow.WorkPriority")]
    public sealed class WorkPriority
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WorkPriority()
        {
            Works = new HashSet<Work>();
            WorkReceivers = new HashSet<WorkReceiver>();
        }

        public long Id { get; set; }

        public int PriorityNumber { get; set; }

        [Required]
        [StringLength(255)]
        public string PriorityTitle { get; set; }

        [Required]
        [StringLength(20)]
        public string PriorityColor { get; set; }

        [Required]
        public byte[] PriorityIcon { get; set; }

        public string Comment { get; set; }

        public bool IsDeleted { get; set; }

        public long? CreatorId { get; set; }

        public DateTime CreateTime { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] TimeTag { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Work> Works { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<WorkReceiver> WorkReceivers { get; set; }
    }
}
