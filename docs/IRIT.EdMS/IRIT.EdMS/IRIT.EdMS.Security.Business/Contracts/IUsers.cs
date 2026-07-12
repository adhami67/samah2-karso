using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using IRIT.EdMS.Security.Model.ViewModel.Account;
using IRIT.Framework.DataModel.Security;

namespace IRIT.EdMS.Security.Business.Contracts
{
    public interface IUsers
    {
        void Add(User user);

        void Edit(User user);

        void Delete(long userId);

        List<User> GetAll();

        User GetById(long userId);

        int Count();

        List<User> Get(Expression<Func<User, bool>> filterExperation);

        AccountDetailsViewModel UserLogin(string userName, string password);

        //List<School> GetSchools(long userId);

        bool CheckUserNameExist(string userName, long? id);
    }
}
