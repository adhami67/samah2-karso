using System.Data.Entity;
using IRIT.Framework.DataModel.Common;

namespace IRIT.Framework.DataAccess.Context
{
    public partial class ApplicationDbContext
    {
        public virtual DbSet<BinaryLargeObject> BinaryLargeObject  { get; set; }
        public virtual DbSet<Department> Department  { get; set; }
        public virtual DbSet<StudyGrade> StudyGrade { get; set; }
        public virtual DbSet<FiscalYear> FiscalYear { get; set; }
        public virtual DbSet<InfraStructureRing> InfraStructureRing { get; set; }
        public virtual DbSet<Lookup> Lookup { get; set; }
        public virtual DbSet<LookupKind> LookupKind { get; set; }
        public virtual DbSet<LookupKindValue> LookupKindValue { get; set; }
        public virtual DbSet<Party> Party { get; set; }
        public virtual DbSet<PartyAddress> PartyAddresses { get; set; }
        public virtual DbSet<PartyOwners> PartyOwners { get; set; }
        public virtual DbSet<Region> Region { get; set; }
        public virtual DbSet<Religion> Religion { get; set; }
        public virtual DbSet<School> School { get; set; }
        public virtual DbSet<SchoolsGroup> SchoolsGroup { get; set; }
        public virtual DbSet<TableDocuments> TableDocuments { get; set; }
        public virtual DbSet<TableExtensions> TableExtensions { get; set; }
        public virtual DbSet<TableExtensionProperties> TableExtensionProperties { get; set; }
        public virtual DbSet<TableExtensionValues> TableExtensionValues { get; set; }
        public virtual DbSet<SolutionVersion> SolutionVersions { get; set; }
    }
}
