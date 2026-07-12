using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using IRIT.EdMS.HumanResources.Business.Contracts;
using IRIT.Framework.DataAccess.Context;
using IRIT.Framework.DataModel.HumanResources;

namespace IRIT.EdMS.HumanResources.Business.Business
{
    public class EmployeeStatuteBl : IEmployeeStatute
    {
        #region Data Member
        private readonly ApplicationDbContext _dbContext;
        private readonly IDbSet<EmployeeStatute> _employeeStatutes;

        public EmployeeStatuteBl(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _employeeStatutes = dbContext.Set<EmployeeStatute>();
        }

        #endregion
        public void Add(EmployeeStatute entity)
        {
            _employeeStatutes.Add(entity);
        }

        public int Count()
        {
            return _employeeStatutes.Count();
        }

        public void Delete(long entityId)
        {
            var employeeStatutes = new EmployeeStatute() { Id = entityId };
            _employeeStatutes.Attach(employeeStatutes);
            _employeeStatutes.Remove(employeeStatutes);
        }

        public void Edit(EmployeeStatute entity)
        {
            _employeeStatutes.Attach(entity);

            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public EmployeeStatute GetById(long entityId)
        {
            return _employeeStatutes.FirstOrDefault(c => c.Id == entityId);
        }

        public List<EmployeeStatute> GetAll()
        {
            return _employeeStatutes.ToList();
        }

        public List<EmployeeStatute> Get(Expression<Func<EmployeeStatute, bool>> filterExperation)
        {
            return _employeeStatutes.Where(filterExperation).ToList();
        }
    }
}
