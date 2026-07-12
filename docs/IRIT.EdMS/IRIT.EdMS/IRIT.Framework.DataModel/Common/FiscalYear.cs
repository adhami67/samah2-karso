using IRIT.Framework.DataModel.Security;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRIT.Framework.DataModel.Common
{
    [Table("FiscalYear", Schema = "Common")]
    public class FiscalYear
    {
        #region Constructor
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FiscalYear()
        {

        }
        #endregion

        #region Data Columns


        [Column("Id", TypeName = "bigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Column("InfraStructureRingId", TypeName = "bigint")]
        //[ForeignKey("FK_FiscalYear_InfraStructureRingId_2_InfraStructureRing")]
        [ForeignKey("InfraStructureRing")]
        public long InfraStructureRingId { get; set; }

        [Column("FiscalYear", TypeName = "int")]
        public int Year { get; set; }

        [Column("FromDate", TypeName = "DateTime")]
        public DateTime FromDate { get; set; }

        [Column("ToDate", TypeName = "DateTime")]
        public DateTime ToDate { get; set; }

        [Column("Comment", TypeName = "nvarchar(max)")]
        public string Comment { get; set; }

        #region Common Data Columns
        [Column("IsDeleted", TypeName = "bit")]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Column("CreatorId", TypeName = "bigint")]
        //[ForeignKey("FK_FiscalYear_CreatorId_2_Users")]
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

        #region Objects that depend on [FiscalYear]

        #endregion

        #region Objects on witch [FiscalYear] depends (ForeinKey Relationship)

        public virtual InfraStructureRing InfraStructureRing { get; set; }

        public virtual User CreatorUser { get; set; }
        #endregion
    }
}
