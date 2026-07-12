using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IRIT.Framework.DataModel.Common;
using System.Collections.Generic;

namespace IRIT.Framework.DataModel.Security
{
    [Table("Users", Schema = "Security")]
    public class User
    {
        #region Constructor
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        //public User()
        //{
        //    UsersGroups = new HashSet<UsersGroup>();
        //    UsersGroupMembers = new HashSet<UsersGroupMember>();
        //    UsersPolicies = new HashSet<UsersPolicy>();

        //}
        #endregion

        #region Data Columns


        [Column("Id", TypeName = "bigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }


        [Column("PartyId", TypeName = "bigint")]
        //[ForeignKey("FK_User_PartyId_2_Party")]
        [ForeignKey("Party")]
        public long PartyId { get; set; }

        [Column("UserName", TypeName = "nvarchar(50)")]
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Column("UserPassword", TypeName = "nvarchar(max)")]
        [Required]
        public string UserPassword { get; set; }

        [Column("IsAdmin", TypeName = "bit")]
        [DefaultValue(false)]
        public bool IsAdmin { get; set; }

        [Column("LastLockoutDate", TypeName = "DateTime")]
        public DateTime? LastLockoutDate { get; set; }

        [Column("LastPasswordChangedDate", TypeName = "DateTime")]
        public DateTime? LastPasswordChangedDate { get; set; }

        [Column("PasswordQuestion", TypeName = "nvarchar(max)")]
        [Required]
        public string PasswordQuestion { get; set; }

        [Column("PasswordAnswer", TypeName = "nvarchar(max)")]
        [Required]
        public string PasswordAnswer { get; set; }

        [Column("FailedLoginCount", TypeName = "int")]
        [DefaultValue(0)]
        public int FailedLoginCount { get; set; }

        [Column("LastFailedLoginDate", TypeName = "DateTime")]
        public DateTime? LastFailedLoginDate { get; set; }

        [Column("FirstPasswordChanged", TypeName = "bit")]
        [DefaultValue(false)]
        public bool FirstPasswordChanged { get; set; }

        [Column("CurrentSchoolId", TypeName = "bigint")]
        [ForeignKey("School")]
        public long? CurrentSchoolId { get; set; }

        [Column("CurrentDepartmentId", TypeName = "bigint")]
        public long? CurrentDepartmentId { get; set; }


        [Column("CurrentFiscalYearId", TypeName = "bigint")]
        public long? CurrentFiscalYearId { get; set; }

        [Column("Comment", TypeName = "nvarchar(max)")]
        public string Comment { get; set; }

        #region Common Data Columns
        [Column("IsDeleted", TypeName = "bit")]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Column("CreatorId", TypeName = "bigint")]
        //[ForeignKey("FK_User_CreatorId_2_Users")]
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

        #region Objects that depend on [User]

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<UsersGroup> UsersGroups { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<UsersGroupMember> UsersGroupMembers { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<UsersPolicy> UsersPolicies { get; set; }
        #endregion

        #region Objects on witch [User] depends (ForeinKey Relationship)
        public virtual Party Party { get; set; }

        public virtual School School { get; set; }

        //public virtual Department CurrentDepartment { get; set; }
        //public virtual FiscalYear CurrentFiscalYear { get; set; }

        public virtual User CreatorUser { get; set; }
        #endregion
    }
}
