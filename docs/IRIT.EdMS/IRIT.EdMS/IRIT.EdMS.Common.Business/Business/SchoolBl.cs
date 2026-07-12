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
    public class SchoolBl : ISchool
    {
        #region Data Member
        private readonly ApplicationDbContext _dbContext;
        private readonly IDbSet<School> _schools;

        public SchoolBl(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _schools = dbContext.Set<School>();
        }
        #endregion

        public IList<School> GetAll()
        {
            return _schools.ToList();
        }

        public void Add(School school)
        {
            _schools.Add(school);
        }

        public School GetById(long schoolId)
        {
            return _schools.FirstOrDefault(c => c.Id == schoolId);
        }

        public void Edit(School school)
        {
            _schools.Attach(school);

            _dbContext.Entry(school).State = EntityState.Modified;
        }

        public void Delete(long schoolId)
        {
            var user = new School() { Id = schoolId };
            _schools.Attach(user);
            _schools.Remove(user);
        }

        public int Count()
        {
            return _schools.Count();
        }

        public List<School> Get(Expression<Func<School, bool>> filterExperation)
        {
            return _schools.Where(filterExperation).ToList();
        }
    }
}
