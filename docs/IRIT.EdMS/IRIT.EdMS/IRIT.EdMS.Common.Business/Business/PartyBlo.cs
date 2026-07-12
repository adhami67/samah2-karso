using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using IRIT.EdMS.Common.Business.Contract;
using IRIT.Framework.DataAccess.Context;
using IRIT.Framework.DataModel.Common;
using IRIT.Framework.Utility.Utility;

namespace IRIT.EdMS.Common.Business.Business
{
    public class PartyBlo : IParty
    {
        #region Data member
        private readonly ApplicationDbContext _dbContext;
        private readonly IDbSet<Party> _partys;
        #endregion

        #region Constructor
        public PartyBlo(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _partys = _dbContext.Set<Party>();
        }
        #endregion

        public void Add(Party party)
        {
            _partys.Add(party);
        }

        public List<Party> Get(Expression<Func<Party, bool>> filterExperation)
        {
            return _partys.Where(filterExperation).ToList();
        }

        public List<Party> GetPartyUser(Expression<Func<Party, bool>> filterExperation)
        {
            return _partys.Where(filterExperation).ToList();
        }

        public int Count()
        {
            return _partys.Count();
        }

        public void Delete(long partyId)
        {
            var party = new Party() { Id = partyId };
            _partys.Attach(party);
            _partys.Remove(party);
        }

        public Party GetById(long partyId)
        {
            return _partys.FirstOrDefault(c => c.Id == partyId);
        }

        public void Edit(Party party)
        {
            _partys.Attach(party);

            _dbContext.Entry(party).State = EntityState.Modified;
        }

        public IList<Party> GetAll()
        {
            return _partys.ToList();
        }

        public bool CheckEmailExist(string email, long? id)
        {
            email = email.FixGmailDots();
            return id == null
               ? _partys.Any(a => a.Email == email.ToLower())
               : _partys.Any(a => a.Email == email.ToLower() && a.Id != id.Value);
        }

        public List<Party> GetPartyEmployee(Expression<Func<Party, bool>> filterExperation)
        {
            return _partys.Where(filterExperation).ToList();
        }
    }
}
