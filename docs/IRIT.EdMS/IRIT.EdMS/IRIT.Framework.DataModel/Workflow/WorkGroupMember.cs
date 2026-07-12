using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRIT.Framework.DataModel.Workflow
{
    [Table("Workflow.WorkGroupMembers")]
    public class WorkGroupMember
    {
        public long Id { get; set; }

        public long WorkGroupId { get; set; }

        public long MemberStatuteId { get; set; }

        public long MemberUserId { get; set; }

        public DateTime? InviteTime { get; set; }

        public DateTime? JoinTime { get; set; }

        public DateTime? LeaveTime { get; set; }

        public bool IsDeleted { get; set; }

        public long? CreatorId { get; set; }

        public DateTime CreateTime { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] TimeTag { get; set; }

        public virtual WorkGroup WorkGroup { get; set; }
    }
}
