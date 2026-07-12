using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using IRIT.EdMS.Common.Business.Contract;
using IRIT.Framework.DataAccess.Context;
using IRIT.Framework.DataModel.Common;

namespace IRIT.EdMS.Common.Business.Business
{
    public class SchoolsGroupBl : ISchoolsGroup
    {
        private ApplicationDbContext _dbContext;
        private IDbSet<SchoolsGroup> _schoolsGroup;

        public SchoolsGroupBl(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _schoolsGroup = dbContext.Set<SchoolsGroup>();
        }

        public void Add(SchoolsGroup entity)
        {
            _schoolsGroup.Add(entity);
        }

        public int Count()
        {
            return _schoolsGroup.Count();
        }

        public void Delete(long entityId)
        {
            var schoolsGroup = new SchoolsGroup() { Id = entityId };
            _schoolsGroup.Attach(schoolsGroup);
            _schoolsGroup.Remove(schoolsGroup);
        }

        public void Edit(SchoolsGroup entity)
        {
            _schoolsGroup.Attach(entity);

            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public SchoolsGroup GetById(long entityId)
        {
            return _schoolsGroup.FirstOrDefault(c => c.Id == entityId);
        }

        public IList<SchoolsGroup> GetAll()
        {
            return _schoolsGroup.ToList();
        }

        public List<SchoolsGroup> Get(Expression<Func<SchoolsGroup, bool>> filterExperation)
        {
            return _schoolsGroup.Where(filterExperation).ToList();
        }
    }
}
