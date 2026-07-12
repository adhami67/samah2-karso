using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using IRIT.Framework.DataModel.Common;
using IRIT.Framework.DataModel.Common.Simple;

namespace IRIT.EdMS.Common.Business.Contract
{
    public interface IPartyOwner
    {
        IList<PartyOwners> GetAll();

        void Add(PartyOwners entity);

        PartyOwners GetById(long entityId);

        List<PartyOwners> Get(Expression<Func<PartyOwners, bool>> filterExperation);

        List<SimplePartyOwner> GetSchoolOwner(long userId, long partyId);

        void Edit(PartyOwners entity);

        void Delete(long entityId);

        int Count();

        IEnumerable<SelectListItem> GetSelectList(long partyId);
    }
}
