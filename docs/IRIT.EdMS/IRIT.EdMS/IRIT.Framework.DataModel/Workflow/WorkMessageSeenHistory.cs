using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRIT.Framework.DataModel.Workflow
{
    [Table("Workflow.WorkMessageSeenHistory")]
    public class WorkMessageSeenHistory
    {
        public long Id { get; set; }

        public long? WorkMessageId { get; set; }

        public long ReceiverStatuteId { get; set; }

        public long ReceiverUserId { get; set; }

        public DateTime? SeenTime { get; set; }

        public string Comment { get; set; }

        public bool IsDeleted { get; set; }

        public long? CreatorId { get; set; }

        public DateTime CreateTime { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] TimeTag { get; set; }

        public virtual WorkMessage WorkMessage { get; set; }
    }
}
