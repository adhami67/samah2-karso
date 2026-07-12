using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRIT.Framework.DataModel.Workflow
{
    [Table("Workflow.WorkAttachments")]
    public class WorkAttachment
    {
        public long Id { get; set; }

     
        public long WorkId { get; set; }

        public long AttachmentKindId { get; set; }

        public string TableName { get; set; }

        public long? EntityId { get; set; }

        public long? AttachmentId { get; set; }

        public string Comment { get; set; }

        public bool IsDeleted { get; set; }

        public long? CreatorId { get; set; }

        public DateTime CreateTime { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] TimeTag { get; set; }
        [ForeignKey("WorkId")]
        public virtual Work Work { get; set; }
    }
}
