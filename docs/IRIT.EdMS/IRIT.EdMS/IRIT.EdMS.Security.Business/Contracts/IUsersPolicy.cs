using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using IRIT.Framework.DataModel.Security;

namespace IRIT.EdMS.Security.Contracts
{
   public interface IUsersPolicy
    {
        IList<UsersPolicy> GetAll();

        void Add(UsersPolicy entity);

        UsersPolicy GetById(long entityId);

        List<UsersPolicy> Get(Expression<Func<UsersPolicy, bool>> filterExperation);

        void Edit(UsersPolicy entity);

        void Delete(long entityId);

        int Count();
    }
}
