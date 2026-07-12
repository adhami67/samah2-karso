using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using IRIT.EdMS.Common.Business.Contract;
using IRIT.Framework.DataAccess.Context;
using IRIT.Framework.DataModel.Common;
using IRIT.Framework.DataModel.Common.Simple;

namespace IRIT.EdMS.Common.Business.Business
{
    public class PartyOwnerBl: IPartyOwner
    {
        private ApplicationDbContext _dbContext;
        private IDbSet<PartyOwners> _partyOwner;
        private IDbSet<School> _school;

        public PartyOwnerBl(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _partyOwner = dbContext.Set<PartyOwners>();
            _school = dbContext.Set<School>();
        }

        public void Add(PartyOwners entity)
        {
            _partyOwner.Add(entity);
        }

        public int Count()
        {
            return _partyOwner.Count();
        }

        public IEnumerable<SelectListItem> GetSelectList(long partyId)
        {
            return _partyOwner.Where(c => c.Id == partyId).ToList().Select(c => new SelectListItem
            {
                // Text = c.
            });
        }

        public void Delete(long entityId)
        {
            var partyOwner = new PartyOwners() { Id = entityId };
            _partyOwner.Attach(partyOwner);
            _partyOwner.Remove(partyOwner);
        }

        public List<SimplePartyOwner> GetSchoolOwner(long userId, long partyId)
        {
            var partyOwners = new List<SimplePartyOwner>();
            if (userId == 1)
            {
                partyOwners.AddRange(_school.Where(c => c.IsDeleted == false)
                    .Select(c => new SimplePartyOwner {Id = c.Id, OwnerTitle = c.SchoolName}).ToList());

                return partyOwners;
            }

            partyOwners.AddRange(
                _partyOwner.Where(
                    c =>
                        c.IsDeleted == false && c.OwnerId != null &&
                        c.OwnerType == Framework.Common.Enum.OwnerTypesEnum.School && c.PartyId == partyId)
                    .Select(c => new SimplePartyOwner
                    {
                        Id = (long) c.OwnerId,
                        OwnerTitle = _school.FirstOrDefault(sc => sc.Id == c.OwnerId).SchoolName
                    }).ToList());

            return partyOwners;
        }

        public void Edit(PartyOwners entity)
        {
            _partyOwner.Attach(entity);

            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public PartyOwners GetById(long entityId)
        {
            return _partyOwner.FirstOrDefault(c => c.Id == entityId);
        }

        public IList<PartyOwners> GetAll()
        {
            return _partyOwner.ToList();
        }

        public List<PartyOwners> Get(Expression<Func<PartyOwners, bool>> filterExperation)
        {
            return _partyOwner.Where(filterExperation).ToList();
        }
    }
}
