using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using IRIT.Framework.DataModel.HumanResources;

namespace IRIT.EdMS.HumanResources.Business.Contracts
{
    public interface IEmployee
    {
        IList<Employee> GetAll();

        void Add(Employee entity);

        Employee GetById(long entityId);

        List<Employee> Get(Expression<Func<Employee, bool>> filterExperation);

        void Edit(Employee entity);

        void Delete(long entityId);

        int Count();
    }
}
