using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRIT.Framework.DataModel.Workflow
{
    [Table("Workflow.WorkPermissions")]
    public class WorkPermission
    {
        public long Id { get; set; }

        public long WorkId { get; set; }

        public bool FinishWorkByReceiver { get; set; }

        public bool FinishWorkByFollower { get; set; }

        public bool SendReadReceipt { get; set; }

        public bool SendWorkChangeStateReceipt { get; set; }

        public bool AllowSeeingPreviousWorkflow { get; set; }

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
