using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using IRIT.Framework.DataModel.Common;

namespace IRIT.EdMS.Common.Business.Contract
{
    public interface IInfraStructureRing
    {
        void Add(InfraStructureRing entity);

        void Edit(InfraStructureRing entity);

        void Delete(long entityId);

        InfraStructureRing GetById(long entityId);

        IList<InfraStructureRing> GetAll();

        List<InfraStructureRing> Get(Expression<Func<InfraStructureRing, bool>> filterExperation);
       
        int Count();
    }

}
