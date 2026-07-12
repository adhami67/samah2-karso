using IRIT.Framework.DataModel.Security;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRIT.Framework.DataModel.Common
{
    [Table("Lookup",Schema = "Common")]
    public  class Lookup
    {
        #region Constructor

     
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Lookup()
        {
            LookupKindValues = new HashSet<LookupKindValue>();
            Parties = new HashSet<Party>();
            Schools = new HashSet<School>();
            TableExtensionProperties = new HashSet<TableExtensionProperties>();
        }
        #endregion

        #region Data Columns

      
        [Column("Id", TypeName = "bigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Column("OwnerEntity", TypeName = "nvarchar(max)")]
        [Required]
        public string OwnerEntity { get; set; }

        [Column("OwnerProperty", TypeName = "nvarchar(max)")]
        [Required]
        public string OwnerProperty { get; set; }

        [Column("Code", TypeName = "nvarchar(50)")]
        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [Column("Title", TypeName = "nvarchar(max)")]
        [Required]
        public string Title { get; set; }

        [Column("Comment", TypeName = "nvarchar(max)")]
        public string Comment { get; set; }

        [Column("TimeTag",TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] TimeTag { get; set; }
        #endregion


        #region Objects that depend on [Lookup]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<LookupKindValue> LookupKindValues { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Party> Parties { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<School> Schools { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<TableExtensionProperties> TableExtensionProperties { get; set; }

       
        #endregion

        #region Objects on witch [Lookup] depends (ForeinKey Relationship)

        public virtual User CreatorUser { get; set; }
        #endregion


       
    }
}
