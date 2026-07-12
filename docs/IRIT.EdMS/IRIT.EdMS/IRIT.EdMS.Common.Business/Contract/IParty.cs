using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using IRIT.Framework.DataModel.Common;

namespace IRIT.EdMS.Common.Business.Contract
{
    public interface IParty
    {
        void Add(Party party);

        void Edit(Party party);

        void Delete(long partyId);

        Party GetById(long partyId);

        IList<Party> GetAll();

        List<Party> Get(Expression<Func<Party, bool>> filterExperation);

        List<Party> GetPartyUser(Expression<Func<Party, bool>> filterExperation);

        List<Party> GetPartyEmployee(Expression<Func<Party, bool>> filterExperation);

        int Count();

        bool CheckEmailExist(string email, long? id);
    }
}
