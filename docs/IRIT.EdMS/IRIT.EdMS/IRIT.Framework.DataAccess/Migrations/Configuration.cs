using System.Data.Entity.Migrations;
using IRIT.Framework.Common.Enum;
using IRIT.Framework.DataAccess.Context;

namespace IRIT.Framework.DataAccess.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            context.Party.Add(new DataModel.Common.Party { PartyType = PartyTypesEnum.Person, FirstName = "مدیر", LastName = "سیستم", CreateTime = System.DateTime.Now, IsDeleted = false});
            base.Seed(context);
        }
    }
}
