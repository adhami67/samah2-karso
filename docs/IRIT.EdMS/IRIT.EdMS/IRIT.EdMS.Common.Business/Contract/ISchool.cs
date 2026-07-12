using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using IRIT.Framework.DataModel.Common;

namespace IRIT.EdMS.Common.Business.Contract
{
    public interface ISchool
    {
        IList<School> GetAll();

        void Add(School school);

        School GetById(long schoolId);

        List<School> Get(Expression<Func<School, bool>> filterExperation);

        void Edit(School school);

        void Delete(long schoolId);

        int Count();
    }
}
