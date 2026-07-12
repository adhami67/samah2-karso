using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using IRIT.EdMS.Security.Business.Contracts;
using IRIT.Framework.DataAccess.Context;
using IRIT.Framework.DataModel.Security;

namespace IRIT.EdMS.Security.Business.Business
{
    public class UsersGroupMembersBl : IUsersGroupMembers
    {
        private ApplicationDbContext _dbContext;
        private IDbSet<UsersGroupMember> _usersGroupMember;

        public UsersGroupMembersBl(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _usersGroupMember = dbContext.Set<UsersGroupMember>();
        }

        public void Add(UsersGroupMember entity)
        {
            _usersGroupMember.Add(entity);
        }

        public int Count()
        {
            return _usersGroupMember.Count();
        }

        public void Delete(long entityId)
        {
            var usersGroupMember = new UsersGroupMember() { Id = entityId };
            _usersGroupMember.Attach(usersGroupMember);
            _usersGroupMember.Remove(usersGroupMember);
        }

        public void Edit(UsersGroupMember entity)
        {
            _usersGroupMember.Attach(entity);

            _dbContext.Entry(entity).State = EntityState.Modified;
        }
        public UsersGroupMember GetById(long entityId)
        {
            return _usersGroupMember.FirstOrDefault(c => c.Id == entityId);
        }
        public IList<UsersGroupMember> GetAll()
        {
            return _usersGroupMember.ToList();

        }
        public List<UsersGroupMember> Get(Expression<Func<UsersGroupMember, bool>> filterExperation)
        {
            return _usersGroupMember.Where(filterExperation).ToList();
        }
    }
}
