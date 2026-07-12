using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRIT.Framework.DataModel.Workflow
{
    [Table("Workflow.WorkReceivers")]
    public class WorkReceiver
    {
        public long Id { get; set; }

        public long WorkId { get; set; }

        public long WorkTypeId { get; set; }

        public long WorkPriorityId { get; set; }

        public long ReceiverStatuteId { get; set; }

        public long ReceiverUserId { get; set; }

        public long WorkStateKindId { get; set; }

        public DateTime? SeenTime { get; set; }

        public DateTime? ReplyTime { get; set; }

        public DateTime? ReplyDeadlineTime { get; set; }

        public DateTime? ForwardTime { get; set; }

        public DateTime? InProgressTime { get; set; }

        public DateTime? DoneTime { get; set; }

        public DateTime? DoneDeadlineTime { get; set; }

        public DateTime? FinishedTime { get; set; }

        public long? FinishedByUserId { get; set; }

        public DateTime? FinishDeadlineTime { get; set; }

        public string PrivateNote { get; set; }

        public bool IsDeleted { get; set; }

        public long? CreatorId { get; set; }

        public DateTime CreateTime { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] TimeTag { get; set; }

        public virtual Work Work { get; set; }

        public virtual WorkPriority WorkPriority { get; set; }

        public virtual WorkType WorkType { get; set; }
    }
}
