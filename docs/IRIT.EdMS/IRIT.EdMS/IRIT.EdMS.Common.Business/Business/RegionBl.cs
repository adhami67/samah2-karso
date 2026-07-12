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
    public class RegionBl : IRegion
    {
        #region Data Member
        private readonly ApplicationDbContext _dbContext;
        private readonly IDbSet<Region> _regions;

        public RegionBl(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _regions = dbContext.Set<Region>();
        }

        #endregion

        public void Add(Region entity)
        {
            _regions.Add(entity);
        }

        public int Count()
        {
            return _regions.Count();
        }

        public void Delete(long entityId)
        {
            var region = new Region() { Id = entityId };
            _regions.Attach(region);
            _regions.Remove(region);
        }

        public void Edit(Region entity)
        {
            _regions.Attach(entity);

            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public Region GetById(long entityId)
        {
            return _regions.FirstOrDefault(c => c.Id == entityId);
        }

        public IList<Region> GetAll()
        {
            return _regions.ToList();
        }

        public List<Region> Get(Expression<Func<Region, bool>> filterExperation)
        {
            return _regions.Where(filterExperation).ToList();
        }

    }
}
