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
    public class BinaryLargeObjectBl : IBinaryLargeObject
    {
        #region Data Member
        private readonly ApplicationDbContext _dbContext;
        private readonly IDbSet<BinaryLargeObject> _binaryLargeObjects;
        #endregion

        #region Constractor
        public BinaryLargeObjectBl(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _binaryLargeObjects = dbContext.Set<BinaryLargeObject>();
        }
        #endregion

        #region Add
        public void Add(BinaryLargeObject entity)
        {
            _binaryLargeObjects.Add(entity);
        }
        #endregion

        #region Edit
        public void Edit(BinaryLargeObject entity)
        {
            _binaryLargeObjects.Attach(entity);

            _dbContext.Entry(entity).State = EntityState.Modified;
        }
        #endregion

        #region Delete
        public void Delete(long entityId)
        {
            var binaryLargeObject = new BinaryLargeObject() { Id = entityId };
            _binaryLargeObjects.Attach(binaryLargeObject);
            _binaryLargeObjects.Remove(binaryLargeObject);
        }
        #endregion

        #region Get
        public List<BinaryLargeObject> Get(Expression<Func<BinaryLargeObject, bool>> filterExperation)
        {
            return _binaryLargeObjects.Where(filterExperation).ToList();
        }

        IList<BinaryLargeObject> IBinaryLargeObject.GetAll()
        {
            return _binaryLargeObjects.ToList();
        }

        public BinaryLargeObject GetById(long entityId)
        {
            return _binaryLargeObjects.FirstOrDefault(c => c.Id == entityId);
        }

        public int Count()
        {
            return _binaryLargeObjects.Count();
        }
        #endregion
    }
}
