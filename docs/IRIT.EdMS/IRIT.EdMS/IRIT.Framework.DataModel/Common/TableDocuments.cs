using IRIT.Framework.DataModel.Security;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRIT.Framework.DataModel.Common
{
    [Table("TableDocuments",Schema = "Common")]
    public class TableDocuments
    {
        #region Constructor
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TableDocuments()
        {
          
        }

        #endregion

        #region  Data Columns
        [Column("Id", TypeName = "bigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Column("TableName", TypeName = "nvarchar(max)")]
        [Required]
        public string TableName { get; set; }

        [Column("BlobId", TypeName = "bigint")]
        public long EntityId { get; set; }

        [Column("BlobId", TypeName = "int")]
        //[ForeignKey("FK_TableDocuments_BlobId_Blob")]
        [ForeignKey("BinaryLargeObject")]
        public long BlobId { get; set; }

        [Column("DocFileName", TypeName = "nvarchar(max)")]
        public string DocFileName { get; set; }

        [Column("DocFileSize", TypeName = "float")]
        public double? DocFileSize { get; set; }

        [Column("DocFileType", TypeName = "nvarchar(max)")]
        public string DocFileType { get; set; }

        [Column("DocFilePageNumbers", TypeName = "int")]
        public int? DocFilePageNumbers { get; set; }

        [Column("DocFileOtherInfo", TypeName = "nvarchar(max)")]
        public string DocFileOtherInfo { get; set; }

        [Column("CreationTime", TypeName = "DateTime")]
        public DateTime? CreationTime { get; set; }

        [Column("LastWriteTime", TypeName = "DateTime")]
        public DateTime? LastWriteTime { get; set; }

        [Column("Comment", TypeName = "nvarchar(max)")]
        public string Comment { get; set; }

        #region Common Data Columns
        [Column("IsDeleted", TypeName = "bit")]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Column("CreatorId", TypeName = "bigint")]
        //[ForeignKey("FK_TableDocuments_CreatorId_2_Users")]
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

        #region Objects that depend on [TableDocuments]



        #endregion

        #region Objects on witch [TableDocuments] depends (ForeinKey Relationship)
        public virtual BinaryLargeObject BinaryLargeObject { get; set; }
        public virtual User CreatorUser { get; set; }
        #endregion
    }
}
