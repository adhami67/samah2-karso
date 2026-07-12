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
    public class OrganizationStructureBl : IOrganizationStructure
    {
        #region Data Member
        private readonly ApplicationDbContext _dbContext;
        private readonly IDbSet<OrganizationStructure> _organizationStructures;

        public OrganizationStructureBl(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _organizationStructures = dbContext.Set<OrganizationStructure>();
        }

        #endregion

        public void Add(OrganizationStructure entity)
        {
            _organizationStructures.Add(entity);
        }

        public int Count()
        {
            return _organizationStructures.Count();
        }

        public void Delete(long entityId)
        {
            var organizationStructure = new OrganizationStructure() { Id = entityId };
            _organizationStructures.Attach(organizationStructure);
            _organizationStructures.Remove(organizationStructure);
        }

        public void Edit(OrganizationStructure entity)
        {
            _organizationStructures.Attach(entity);

            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public List<OrganizationStructure> Get(Expression<Func<OrganizationStructure, bool>> filterExperation)
        {

            return _organizationStructures.Where(filterExperation).ToList();
        }

        public OrganizationStructure GetById(long entityId)
        {
            return _organizationStructures.FirstOrDefault(c => c.Id == entityId);
        }

        public List<OrganizationStructure> GetAll()
        {
            return _organizationStructures.ToList();
        }
    }
}
