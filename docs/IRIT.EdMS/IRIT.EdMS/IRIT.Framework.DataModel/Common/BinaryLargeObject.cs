using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IRIT.Framework.DataModel.Security;

namespace IRIT.Framework.DataModel.Common
{
    [Table("BinaryLargeObject",Schema = "Common")]
    public class BinaryLargeObject
    {
        #region Constructor
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BinaryLargeObject()
        {
            
        }

        #endregion

        #region Data Columns
        //[Column("Id", TypeName = "bigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

      
        //[Column("Blob", TypeName = "varbinary(max)")]
        public byte[] Blob { get; set; }

        #region Common  Data Columns
        //[Column("IsDeleted", TypeName = "bit")]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Column("CreatorId", TypeName = "bigint")]
        //[ForeignKey("FK_BinaryLargeObject_CreatorId_2_Users")]
        public long CreatorId { get; set; }

        private DateTime? createTime;
        [Column("CreateTime", TypeName = "DateTime")]
        public DateTime CreateTime
        {
            get
            {
                return createTime ?? DateTime.Now;
            }

            set { createTime = value; }
        }

        [Column("TimeTag", TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] TimeTag { get; set; }

        #endregion
        #endregion

        #region Objects that depend on [BinaryLargeObject]

        #endregion

        #region Objects on witch [BinaryLargeObject] depends (ForeinKey Relationship)
    
        public virtual User CreatorUser { get; set; }
        #endregion
    }
}
