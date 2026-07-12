using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRIT.Framework.DataModel.Workflow
{
    [Table("Workflow.WorkGroups")]
    public sealed class WorkGroup
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WorkGroup()
        {
            Works = new HashSet<Work>();
            WorkGroupMembers = new HashSet<WorkGroupMember>();
        }

        public long Id { get; set; }

        public long OwnerStatuteId { get; set; }

        public long OwnerUserId { get; set; }

        public long WorkGroupStateKindId { get; set; }

        [Required]
        [StringLength(255)]
        public string WorkGroupName { get; set; }

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
        public ICollection<WorkGroupMember> WorkGroupMembers { get; set; }
    }
}
