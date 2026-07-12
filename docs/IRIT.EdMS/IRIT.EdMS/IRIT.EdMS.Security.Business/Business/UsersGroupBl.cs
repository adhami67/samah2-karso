using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using IRIT.EdMS.Security.Business.Contracts;
using IRIT.EdMS.Security.Business.Filters;
using IRIT.Framework.DataAccess.Context;
using IRIT.Framework.DataModel.Security;

namespace IRIT.EdMS.Security.Business.Business
{
    public class UsersGroupBl : IUsersGroup
    {
        #region Data Member
        private readonly ApplicationDbContext _dbContext;
        private readonly IDbSet<UsersGroup> _usersGroup;
        #endregion

        public UsersGroupBl(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _usersGroup = _dbContext.Set<UsersGroup>();
            //_mappingEngine = mapper;
        }

        public void Add(UsersGroup userGroup)
        {
            _usersGroup.Add(userGroup);
        }

        public int Count()
        {
            return _usersGroup.Count();
        }

        public void Delete(long userGroupId)
        {
            var userGroup = _usersGroup.FirstOrDefault(c => c.Id == userGroupId);
            _usersGroup.Attach(userGroup);
            _usersGroup.Remove(userGroup);
        }

        public void Edit(UsersGroup userGroup)
        {
            _usersGroup.Attach(userGroup);

            _dbContext.Entry(userGroup).State = EntityState.Modified;
        }

        public List<UsersGroup> Get(Expression<Func<UsersGroup, bool>> filterExperation)
        {
            return _usersGroup.Where(filterExperation).ToList();
        }

        public UsersGroup GetById(long userGroupId)
        {
            return _usersGroup.FirstOrDefault(c => c.Id == userGroupId && c.IsDeleted == false);
        }

        public List<UsersGroup> GetAll()
        {
            return _usersGroup.ToList();
        }

        public IEnumerable<SelectListItem> GetAllAsSelectList()
        {
            _dbContext.EnableFiltering(UserGroupFilters.ActiveList);

            var userGroups = _usersGroup.AsNoTracking().Project().To<SelectListItem>().ToList();

            return userGroups;
        }
    }
}
