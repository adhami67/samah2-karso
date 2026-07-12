using System.Data.Entity;
using IRIT.Framework.DataModel.HumanResources;

namespace IRIT.Framework.DataAccess.Context
{
    public partial class ApplicationDbContext
    {
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeeEducationHistory> EmployeeEducationHistories { get; set; }
        public virtual DbSet<EmployeeFamilyMembers> EmployeeFamilyMembers { get; set; }
        public virtual DbSet<EmployeeJobHistory> EmployeeJobHistories { get; set; }
        public virtual DbSet<EmployeeStatute> EmployeeStatutes { get; set; }
        public virtual DbSet<OrganizationStructure> OrganizationStructures { get; set; }
    }
}
