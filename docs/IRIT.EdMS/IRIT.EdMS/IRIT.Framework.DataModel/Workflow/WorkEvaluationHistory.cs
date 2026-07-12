using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRIT.Framework.DataModel.Workflow
{
    [Table("Workflow.WorkEvaluationHistory")]
    public class WorkEvaluationHistory
    {
        public long Id { get; set; }

        public long WorkId { get; set; }

        public long EvaluationItemId { get; set; }

        public long EvaluatedByStatuteId { get; set; }

        public long EvaluatedByUserId { get; set; }

        public DateTime EvaluateTime { get; set; }

        public string Comment { get; set; }

        public bool IsDeleted { get; set; }

        public long? CreatorId { get; set; }

        public DateTime CreateTime { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] TimeTag { get; set; }

        public virtual EvaluationItem EvaluationItem { get; set; }

        public virtual Work Work { get; set; }
    }
}
