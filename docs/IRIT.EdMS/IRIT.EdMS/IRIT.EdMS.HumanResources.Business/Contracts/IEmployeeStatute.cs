using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using IRIT.Framework.DataModel.HumanResources;

namespace IRIT.EdMS.HumanResources.Business.Contracts
{
    public interface IEmployeeStatute
    {
        void Add(EmployeeStatute entity);

        void Edit(EmployeeStatute entity);

        void Delete(long entityId);

        List<EmployeeStatute> GetAll();

        List<EmployeeStatute> Get(Expression<Func<EmployeeStatute, bool>> filterExperation);

        EmployeeStatute GetById(long entityId);

        int Count();
    }
}
