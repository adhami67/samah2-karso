using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRIT.Framework.DataModel.Security
{
    [Table("UsersPolicy",Schema = "Security")]
    public class UsersPolicy
    {
        #region Constructor


        #endregion

        #region Data Columns
        [Column("Id", TypeName = "bigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }


        [Column("UserId", TypeName = "bigint")]
        //[ForeignKey("FK_UsersPolicy_UserId_2_Users")]
        [ForeignKey("User")]
        public long UserId { get; set; }


        [Column("UsersGroupId", TypeName = "bigint")]
        //[ForeignKey("FK_UsersPolicy_UsersGroupId_2_UsersGroup")]
        [ForeignKey("UsersGroup")]
        public long? UsersGroupId { get; set; }

        [Column("AccessibleResourcesId", TypeName = "bigint")]
        //[ForeignKey("FK_UsersPolicy_AccessibleResourcesId_2_UsersGroup")]
        [ForeignKey("AccessibleResource")]
        public long AccessibleResourcesId { get; set; }

        public long AccessTypeId { get; set; }

        [Column("Comment", TypeName = "nvarchar(max)")]
        public string Comment { get; set; }

        #region Common Data Columns
        [Column("IsDeleted", TypeName = "bit")]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Column("CreatorId", TypeName = "bigint")]
        //[ForeignKey("FK_UsersPolicy_CreatorId_2_Users")]
        public long CreatorId { get; set; }

        private DateTime? _createTime;
        [Column("CreateTime", TypeName = "DateTime")]
        public DateTime CreateTime
        {
            get
            {
                return _createTime ?? DateTime.Now;
            }

             set { _createTime = value; }
        }

        [Column("TimeTag", TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] TimeTag { get; set; }

        #endregion
        #endregion

        #region Objects that depend on [UsersPolicy]

        #endregion

        #region Objects on witch [UsersPolicy] depends (ForeinKey Relationship)
        public virtual User User { get; set; }

        public virtual UsersGroup UsersGroup { get; set; }

        public virtual AccessibleResource AccessibleResource { get; set; }

        public virtual User CreatorUser { get; set; }
        #endregion
    }
}
