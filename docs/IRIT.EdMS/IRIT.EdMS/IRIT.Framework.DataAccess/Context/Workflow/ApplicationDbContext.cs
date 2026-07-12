using System.Data.Entity;
using IRIT.Framework.DataModel.Workflow;

namespace IRIT.Framework.DataAccess.Context
{
    public partial class ApplicationDbContext
    {
        public virtual DbSet<EvaluationItem> EvaluationItems { get; set; }
        public virtual DbSet<Work> Works { get; set; }
        public virtual DbSet<WorkAttachment> WorkAttachments { get; set; }
        public virtual DbSet<WorkEvaluationHistory> WorkEvaluationHistories { get; set; }
        public virtual DbSet<WorkGroupMember> WorkGroupMembers { get; set; }
        public virtual DbSet<WorkGroup> WorkGroups { get; set; }
        public virtual DbSet<WorkMessage> WorkMessages { get; set; }
        public virtual DbSet<WorkMessageSeenHistory> WorkMessageSeenHistories { get; set; }
        public virtual DbSet<WorkPermission> WorkPermissions { get; set; }
        public virtual DbSet<WorkPriority> WorkPriorities { get; set; }
        public virtual DbSet<WorkReceiver> WorkReceivers { get; set; }
        public virtual DbSet<WorkStatusChangeHistory> WorkStatusChangeHistories { get; set; }
        public virtual DbSet<WorkType> WorkTypes { get; set; }
    }
}
