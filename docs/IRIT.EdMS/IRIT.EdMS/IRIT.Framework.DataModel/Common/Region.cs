using IRIT.Framework.DataModel.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRIT.Framework.DataModel.Common
{
    [Table("Region",Schema ="Common")]
    public  class Region
    {
        #region Constructor
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Region()
        {
            Parties = new HashSet<Party>();
            Regions = new HashSet<Region>();
            
        }
        #endregion

        #region  Data Columns
        [Column("Id", TypeName = "bigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Column("RegionType", TypeName = "bigint")]
        public long? RegionType { get; set; }

        [Column("ParentId", TypeName = "bigint")]
        //[ForeignKey("FK_Region_ParentId_2_Region")]
        [ForeignKey("Parent")]
        public long? ParentId { get; set; }

        [Column("Code", TypeName = "nvarchar(50)")]
        [StringLength(50)]
        public string Code { get; set; }

        [Column("Title", TypeName = "nvarchar(255)")]
        [Required]
        [StringLength(255)]
        public string Title { get; set; }

       

        [Column("Comment", TypeName = "nvarchar(max)")]
        public string Comment { get; set; }

        #region Common Data Columns
        [Column("IsDeleted", TypeName = "bit")]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Column("CreatorId", TypeName = "bigint")]
        //[ForeignKey("FK_Region_CreatorId_2_Users")]
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


        #region Objects that depend on [Region]

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Region> Regions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Party> Parties { get; set; }


        #endregion

        #region Objects on witch [Region] depends (ForeinKey Relationship)
        public Region Parent { get; set; }

        public virtual User CreatorUser { get; set; }
        #endregion

      
     
    
    }
}
