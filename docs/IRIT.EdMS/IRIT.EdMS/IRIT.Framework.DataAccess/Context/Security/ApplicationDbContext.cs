using System.Data.Entity;
using IRIT.Framework.DataModel.Security;

namespace IRIT.Framework.DataAccess.Context
{
    public partial class ApplicationDbContext
    {
        public virtual DbSet<AccessibleResource> AccessibleResources { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UsersGroup> UsersGroups { get; set; }
        public virtual DbSet<UsersGroupMember> UsersGroupMembers { get; set; }
        public virtual DbSet<UsersPolicy> UsersPolicies { get; set; }
    }
}
