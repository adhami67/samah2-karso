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
    public class EmployeeBl : IEmployee
    {
        #region Data Member
        private readonly ApplicationDbContext _dbContext;
        private readonly IDbSet<Employee> _employee;

        public EmployeeBl(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _employee = dbContext.Set<Employee>();
        }

        #endregion
        public void Add(Employee entity)
        {
            _employee.Add(entity);
        }

        public int Count()
        {
            return _employee.Count();
        }

        public void Delete(long entityId)
        {
            var employee = new Employee() { Id = entityId };
            _employee.Attach(employee);
            _employee.Remove(employee);
        }

        public void Edit(Employee entity)
        {
            _employee.Attach(entity);

            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public Employee GetById(long entityId)
        {
            return _employee.FirstOrDefault(c => c.Id == entityId);
        }

        public IList<Employee> GetAll()
        {
            return _employee.ToList();
        }

        public List<Employee> Get(Expression<Func<Employee, bool>> filterExperation)
        {
            return _employee.Where(filterExperation).ToList();
        }
    }
}
