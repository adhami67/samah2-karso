using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using IRIT.EdMS.Common.Business.Contract.Lookup;
using IRIT.Framework.DataAccess.Context;
using IRIT.Framework.DataModel.Common;

namespace IRIT.EdMS.Common.Business.Business.Lookup
{
    public class GradeKindBl : IGradeKind
    {
        #region Data Member
        private readonly ApplicationDbContext _dbContext;
        private readonly IDbSet<Framework.DataModel.Common.Lookup> _lookups;
        private readonly string OwnerPropertyName = "GradeKind";

        public GradeKindBl(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _lookups = dbContext.Set<Framework.DataModel.Common.Lookup>();
        }

        #endregion

        public IList<Framework.DataModel.Common.Lookup> GetAll()
        {
            return _lookups.Where(c => c.OwnerProperty == OwnerPropertyName).ToList();
        }

        public Framework.DataModel.Common.Lookup GetById(long entityId)
        {
            return _lookups.FirstOrDefault(c => c.Id == entityId && c.OwnerProperty == OwnerPropertyName);
        }
    }
}
