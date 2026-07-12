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
    public class InfraStructureRingBl : IInfraStructureRing
    {
        #region Data Member
        private ApplicationDbContext _dbContext;
        private IDbSet<InfraStructureRing> _infraStructureRing;
        #endregion

        #region Constractor
        public InfraStructureRingBl(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _infraStructureRing = dbContext.Set<InfraStructureRing>();
        }
        #endregion

        #region Add
        public void Add(InfraStructureRing entity)
        {
            _infraStructureRing.Add(entity);
        }

        #endregion

        #region Edit
        public void Edit(InfraStructureRing entity)
        {
            _infraStructureRing.Attach(entity);

            _dbContext.Entry(entity).State = EntityState.Modified;
        }
        #endregion

        #region Delete
        public void Delete(long entityId)
        {
            var infraStructureRing = new InfraStructureRing() { Id = entityId };
            _infraStructureRing.Attach(infraStructureRing);
            _infraStructureRing.Remove(infraStructureRing);
        }
        #endregion

        #region Get
        public InfraStructureRing GetById(long entityId)
        {
            try
            {
                return _infraStructureRing.FirstOrDefault(c => c.Id == entityId);
            }
            catch (Exception exp)
            {
                throw;
            }
        }

        public IList<InfraStructureRing> GetAll()
        {
            return _infraStructureRing.ToList();
        }

        public List<InfraStructureRing> Get(Expression<Func<InfraStructureRing, bool>> filterExperation)
        {
            return _infraStructureRing.Where(filterExperation).ToList();
        }

        public int Count()
        {
            return _infraStructureRing.Count();
        }
        #endregion
    }
}
