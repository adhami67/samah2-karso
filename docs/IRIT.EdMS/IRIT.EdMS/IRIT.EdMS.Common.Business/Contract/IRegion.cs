using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using IRIT.Framework.DataModel.Common;

namespace IRIT.EdMS.Common.Business.Contract
{
    public interface IRegion
    {
        IList<Region> GetAll();

        void Add(Region entity);

        Region GetById(long entityId);

        List<Region> Get(Expression<Func<Region, bool>> filterExperation);

        void Edit(Region entity);

        void Delete(long entityId);

        int Count();
    }
}
