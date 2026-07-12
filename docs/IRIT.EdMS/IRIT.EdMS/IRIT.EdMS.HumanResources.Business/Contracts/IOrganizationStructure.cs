using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using IRIT.Framework.DataModel.HumanResources;

namespace IRIT.EdMS.HumanResources.Business.Contracts
{
    public interface IOrganizationStructure
    {
        void Add(OrganizationStructure entity);

        void Edit(OrganizationStructure entity);

        void Delete(long entityId);

        List<OrganizationStructure> GetAll();
        
        List<OrganizationStructure> Get(Expression<Func<OrganizationStructure, bool>> filterExperation);

        OrganizationStructure GetById(long entityId);

        int Count();
    }
}
