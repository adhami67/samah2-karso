using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using IRIT.Framework.DataModel.Common;

namespace IRIT.EdMS.Common.Business.Contract
{
    public interface ISchoolsGroup
    {
        IList<SchoolsGroup> GetAll();

        void Add(SchoolsGroup entity);

        SchoolsGroup GetById(long entityId);

        List<SchoolsGroup> Get(Expression<Func<SchoolsGroup, bool>> filterExperation);

        void Edit(SchoolsGroup entity);

        void Delete(long entityId);

        int Count();
    }
}
