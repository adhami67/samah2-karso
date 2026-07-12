using System.Data.Entity;
using IRIT.Framework.DataModel.License;

namespace IRIT.Framework.DataAccess.Context
{
   public partial class ApplicationDbContext 
    {
        public virtual DbSet<Rent> Rents { get; set; }
        public virtual DbSet<Renter> Renters { get; set; }
    }
}
