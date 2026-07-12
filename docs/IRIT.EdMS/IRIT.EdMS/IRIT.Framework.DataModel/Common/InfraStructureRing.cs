using IRIT.Framework.DataModel.License;
using IRIT.Framework.DataModel.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRIT.Framework.DataModel.Common
{
    [Table("InfraStructureRing", Schema ="Common")]
    public  class InfraStructureRing
    {

        #region Constructor
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InfraStructureRing()
        {
          
            FiscalYears = new HashSet<FiscalYear>();
            Schools = new HashSet<School>();
            SchoolsGroups = new HashSet<SchoolsGroup>();
            Departments = new HashSet<Department>();
            TableExtensions = new HashSet<TableExtensions>();
        }
        #endregion

        #region Data Columns


        [Column("Id", TypeName = "bigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }


        [Column("RenterId", TypeName = "bigint")]
        //[ForeignKey("FK_InfraStructureRing_RenterId_2_Renter")]
        [ForeignKey("Renter")]
        public long? RenterId { get; set; }

        [Column("InfraStructureRingType", TypeName = "bigint")]
        public long InfraStructureRingType { get; set; }

        [Column("Code", TypeName = "nvarchar(50)")]
        [StringLength(50)]
        public string Code { get; set; }

        [Column("Name", TypeName = "nvarchar(255)")]
        [StringLength(255)]
        public string InfraStructureRingName { get; set; }


        [Column("Comment", TypeName = "nvarchar(max)")]
        public string Comment { get; set; }

        #region Common Data Columns
        [Column("IsDeleted", TypeName = "bit")]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Column("CreatorId", TypeName = "bigint")]
        //[ForeignKey("FK_InfraStructureRing_CreatorId_2_Users")]
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

        #region Objects that depend on [InfraStructureRing]
      
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<FiscalYear> FiscalYears { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<School> Schools { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<SchoolsGroup> SchoolsGroups { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Department> Departments { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<TableExtensions> TableExtensions { get; set; }
        #endregion

        #region Objects on witch [InfraStructureRing] depends (ForeinKey Relationship)
        public virtual Renter Renter { get; set; }
   
    
        public virtual User CreatorUser { get; set; }
        #endregion



     
    }
}
