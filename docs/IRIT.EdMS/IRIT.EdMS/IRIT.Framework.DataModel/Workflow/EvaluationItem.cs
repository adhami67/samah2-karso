using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IRIT.Framework.DataModel.Security;

namespace IRIT.Framework.DataModel.Workflow
{
    [Table("EvaluationItem", Schema = "Workflow")]
    public sealed class EvaluationItem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EvaluationItem()
        {
            WorkEvaluationHistories = new HashSet<WorkEvaluationHistory>();
        }

        [Column("ID", TypeName = "bigint")]
        [Key]
        public long Id { get; set; }

        [Column("Code", TypeName = "nvarchar(50)")]
        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [Column("Title", TypeName = "nvarchar(255)")]
        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Column("ItemColor", TypeName = "nvarchar(20)")]
        [Required]
        [StringLength(20)]
        public string ItemColor { get; set; }

        [Column("ItemIcon", TypeName = "varbinary(max)")]
        public byte[] ItemIcon { get; set; }

        [Column("ItemStandardValue", TypeName = "int")]
        public int ItemStandardValue { get; set; }

        [Column("Comment", TypeName = "nvarchar(max)")]
        public string Comment { get; set; }

        [Column("IsDeleted", TypeName = "bit")]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Column("CreatorId", TypeName = "bigint")]
        //[ForeignKey("FK_EvaluationItem_CreatorId_2_Users")]
        public long CreatorId { get; set; }

        public User CreatedBy { get; set; }

        private DateTime? _createTime;
        public DateTime CreateTime
        {
            get
            {
                return _createTime ?? DateTime.Now;
            }

            private set { _createTime = value; }
        }


        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] TimeTag { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<WorkEvaluationHistory> WorkEvaluationHistories { get; set; }
    }
}
