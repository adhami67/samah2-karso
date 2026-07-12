using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using IRIT.EdMS.Security.Business.Contracts;
using IRIT.EdMS.Security.Model.ViewModel.Account;
using IRIT.Framework.Common.Enum;
using IRIT.Framework.DataAccess.Context;
using IRIT.Framework.DataModel.Security;

namespace IRIT.EdMS.Security.Business.Business
{
    public class UsersBl : IUsers
    {
        #region Data Member
        private readonly ApplicationDbContext _dbContext;
        private readonly IDbSet<User> _users;
        #endregion

        public UsersBl(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _users = _dbContext.Set<User>();
        }

        public void Add(User user)
        {
            _users.Add(user);
        }

        public int Count()
        {
            return _users.Count();
        }

        public List<User> Get(Expression<Func<User, bool>> filterExperation)
        {
            return _users.Where(filterExperation).ToList();
        }

        public AccountDetailsViewModel UserLogin(string userName, string password)
        {
            var accountDetails = new AccountDetailsViewModel();
            try
            {
                var user = _users.Where(c => c.UserName == userName && c.UserPassword == password).ToList();

                if (user.Any())
                    return user.Select(c => new AccountDetailsViewModel
                    {
                        Status = LoginStatus.Success,
                        //FullName = $"{c.FirstName} {c.LastName}",
                        UserId = c.Id,
                        PartyId = c.PartyId,
                        //SmallImage = c.SmallImage
                    }).FirstOrDefault();

                accountDetails.Status = LoginStatus.RequiresVerification;
                return accountDetails;
            }
            catch (Exception exp)
            {
                accountDetails.Status = LoginStatus.Failure;
                return accountDetails;
            }
        }

        //public List<School> GetSchools(long userId)
        //{
        //    return _vwUserSchools.Where(c => c.UserID == userId).ToList();
        //}

        public void Delete(long userId)
        {
            var user = new User() { Id = userId };
            _users.Attach(user);
            _users.Remove(user);
        }

        public void Edit(User user)
        {
            _users.Attach(user);

            _dbContext.Entry(user).State = EntityState.Modified;
        }

        public User GetById(long userId)
        {
            return _users.FirstOrDefault(c => c.Id == userId);
        }

        public List<User> GetAll()
        {
            return _users.ToList();
        }

        public bool CheckUserNameExist(string userName, long? id)
        {
            return id == null
                ? _users.Any(a => a.UserName == userName.ToLower())
                : _users.Any(a => a.UserName == userName.ToLower() && a.Id != id.Value);
        }
    }
}
