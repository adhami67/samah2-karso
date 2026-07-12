using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using IRIT.Framework.DataModel.Workflow;

namespace IRIT.EdMS.Workflow.Business.Contracts
{
    public interface IWorkType
    {
        IList<WorkType> GetAll();

        void Add(WorkType workType);

        WorkType GetById(long workTypeId);

        List<WorkType> Get(Expression<Func<WorkType, bool>> filterExperation);

        void Edit(WorkType workType);

        void Delete(long workTypeId);

        int Count();
    }
}