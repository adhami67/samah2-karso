using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IRIT.EdMS.Common.Business.Contract.Lookup
{
    public interface IGradeKind
    {
        IList<Framework.DataModel.Common.Lookup> GetAll();

        Framework.DataModel.Common.Lookup GetById(long entityId);
    }
}
