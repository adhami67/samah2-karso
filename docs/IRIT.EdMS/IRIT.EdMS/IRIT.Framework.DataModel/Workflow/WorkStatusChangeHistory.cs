using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRIT.Framework.DataModel.Workflow
{
    [Table("Workflow.WorkStatusChangeHistory")]
    public class WorkStatusChangeHistory
    {
        public long Id { get; set; }

        public long WorkId { get; set; }

        public long WorkStatusKindId { get; set; }

        public DateTime WorkStatusTime { get; set; }

        public long WorkStatusChangeByStatuteId { get; set; }

        public long WorkStatusChangeByUserId { get; set; }

        public string Comment { get; set; }

        public bool IsDeleted { get; set; }

        public long? CreatorId { get; set; }

        public DateTime CreateTime { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] TimeTag { get; set; }

        public virtual Work Work { get; set; }
    }
}
