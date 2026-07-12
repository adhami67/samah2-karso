using IRIT.Framework.DataModel.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRIT.Framework.DataModel.Common
{
    [Table("LookupKind", Schema = "Common")]
    public class LookupKind
    {
        #region Constructor


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LookupKind()
        {

        }
        #endregion

        #region Data Columns


        [Column("Id", TypeName = "bigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Column("SchoolId", TypeName = "bigint")]
        //[ForeignKey("FK_LookupKind_SchoolId_2_School")]
        [ForeignKey("School")]
        public long? SchoolId { get; set; }

        [Column("Code", TypeName = "nvarchar(50)")]
        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [Column("Title", TypeName = "nvarchar(max)")]
        [Required]
        public string Title { get; set; }


        [Column("Comment", TypeName = "nvarchar(max)")]
        public string Comment { get; set; }

        #region Common Data Columns
        [Column("IsDeleted", TypeName = "bit")]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Column("CreatorId", TypeName = "bigint")]
        //[ForeignKey("FK_Department_CreatorId_2_Users")]
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


        #region Objects that depend on [LookupKind]

       


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<LookupKindValue> LookupKindValue { get; set; }
        #endregion

        #region Objects on witch [LookupKind] depends (ForeinKey Relationship)
     
        public virtual School School { get; set; }

        public virtual User CreatorUser { get; set; }
        #endregion
      
    }
}
