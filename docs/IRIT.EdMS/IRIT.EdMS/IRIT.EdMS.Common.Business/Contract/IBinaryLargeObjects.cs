using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using IRIT.Framework.DataModel.Common;

namespace IRIT.EdMS.Common.Business.Contract
{
    public interface IBinaryLargeObject
    {
        void Add(BinaryLargeObject entity);

        void Edit(BinaryLargeObject entity);

        void Delete(long entityId);

        BinaryLargeObject GetById(long entityId);

        List<BinaryLargeObject> Get(Expression<Func<BinaryLargeObject, bool>> filterExperation);

        IList<BinaryLargeObject> GetAll();

        int Count();
    }
}
