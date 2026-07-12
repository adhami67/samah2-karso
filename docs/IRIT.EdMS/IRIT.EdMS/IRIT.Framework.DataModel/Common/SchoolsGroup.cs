using IRIT.Framework.DataModel.License;
using IRIT.Framework.DataModel.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRIT.Framework.DataModel.Common
{
    [Table("SchoolsGroup", Schema = "Common")]
    public  class SchoolsGroup
    {
        #region Constructor
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SchoolsGroup()
        {
            Schools = new HashSet<School>();
        }

        #endregion

        #region  Data Columns
        [Column("Id", TypeName = "bigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Column("RenterId", TypeName = "bigint")]
        //[ForeignKey("FK_SchoolsGroup_RenterId_2_Renter")]
        [ForeignKey("Renter")]
        public long? RenterId { get; set; }

        [Column("InfraStructureRingId", TypeName = "bigint")]
        //[ForeignKey("FK_SchoolsGroup_InfraStructureRingId_2_InfraStructureRing")]
        [ForeignKey("InfraStructureRing")]
        public long InfraStructureRingId { get; set; }


        [Column("Code", TypeName = "nvarchar(50)")]
        [StringLength(50)]
        public string Code { get; set; }

        [Column("SchoolsGroupName", TypeName = "nvarchar(255)")]
        [Required]
        [StringLength(255)]
        public string SchoolsGroupName { get; set; }



        [Column("Comment", TypeName = "nvarchar(max)")]
        public string Comment { get; set; }

        #region Common Data Columns
        [Column("IsDeleted", TypeName = "bit")]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Column("CreatorId", TypeName = "bigint")]
        //[ForeignKey("FK_Grade_CreatorId_2_Users")]
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

        #region Objects that depend on [SchoolsGroup]

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<School> Schools { get; set; }


        #endregion

        #region Objects on witch [SchoolsGroup] depends (ForeinKey Relationship)
        public virtual Renter Renter { get; set; }
        public InfraStructureRing InfraStructureRing { get; set; }
        public virtual User CreatorUser { get; set; }
        #endregion



   

      
    }
}
