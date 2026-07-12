using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using IRIT.EdMS.Security.Contracts;
using IRIT.Framework.DataAccess.Context;
using IRIT.Framework.DataModel.Security;

namespace IRIT.EdMS.Security.Business.Business
{
    public class UsersPolicyBl : IUsersPolicy
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IDbSet<UsersPolicy> _usersPolicy;

        public UsersPolicyBl(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _usersPolicy = dbContext.Set<UsersPolicy>();
        }
        public void Add(UsersPolicy entity)
        {
            _usersPolicy.Add(entity);
        }
        public int Count()
        {
            return _usersPolicy.Count();
        }
        public void Delete(long entityId)
        {
            var usersPolicy = new UsersPolicy() { Id = entityId };
            _usersPolicy.Attach(usersPolicy);
            _usersPolicy.Remove(usersPolicy);
        }
        public void Edit(UsersPolicy entity)
        {
            _usersPolicy.Attach(entity);

            _dbContext.Entry(entity).State = EntityState.Modified;
        }
        public UsersPolicy GetById(long entityId)
        {
            return _usersPolicy.FirstOrDefault(c => c.Id == entityId);
        }
        public IList<UsersPolicy> GetAll()
        {
            return _usersPolicy.ToList();
        }
        public List<UsersPolicy> Get(Expression<Func<UsersPolicy, bool>> filterExperation)
        {
            return _usersPolicy.Where(filterExperation).ToList();
        }
    }
}
