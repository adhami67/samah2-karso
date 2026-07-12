using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using IRIT.Framework.DataModel.Security;

namespace IRIT.EdMS.Security.Business.Contracts
{
    public interface IUsersGroupMembers
    {
        IList<UsersGroupMember> GetAll();

        void Add(UsersGroupMember entity);

        UsersGroupMember GetById(long entityId);

        List<UsersGroupMember> Get(Expression<Func<UsersGroupMember, bool>> filterExperation);

        void Edit(UsersGroupMember entity);

        void Delete(long entityId);

        int Count();
    }
}
