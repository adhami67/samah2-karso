using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using IRIT.Framework.DataModel.Security;

namespace IRIT.EdMS.Security.Business.Contracts
{
    public interface IUsersGroup
    {
        void Add(UsersGroup userGroup);

        void Edit(UsersGroup userGroup);

        void Delete(long userGroupId);

        List<UsersGroup> GetAll();

        List<UsersGroup> Get(Expression<Func<UsersGroup, bool>> filterExperation);

        UsersGroup GetById(long userGroupId);

        int Count();

        IEnumerable<SelectListItem> GetAllAsSelectList();
    }
}
