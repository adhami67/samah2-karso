using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using IRIT.EdMS.Workflow.Business.Contracts;
using IRIT.Framework.DataAccess.Context;
using IRIT.Framework.DataModel.Workflow;

namespace IRIT.EdMS.Workflow.Business.Business
{
    public class WorkTypeBl : IWorkType
    {
        private ApplicationDbContext _dbContext;
        private IDbSet<WorkType> _workType;

        public WorkTypeBl(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _workType = dbContext.Set<WorkType>();
        }

        public void Add(WorkType entity)
        {
            _workType.Add(entity);
        }

        public int Count()
        {
            return _workType.Count();
        }

        public IEnumerable<SelectListItem> GetSelectList(long workTypeId)
        {
            return _workType.Where(c => c.Id == workTypeId).ToList().Select(c => new SelectListItem
            {
                // Text = c.
            });
        }

        public void Delete(long entityId)
        {
            var workType = new WorkType() { Id = entityId };
            _workType.Attach(workType);
            _workType.Remove(workType);
        }

        public void Edit(WorkType entity)
        {
            _workType.Attach(entity);

            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public WorkType GetById(long entityId)
        {
            return _workType.FirstOrDefault(c => c.Id == entityId);
        }

        public IList<WorkType> GetAll()
        {
            return _workType.ToList();
        }

        public List<WorkType> Get(Expression<Func<WorkType, bool>> filterExperation)
        {
            return _workType.Where(filterExperation).ToList();
        }
    }
}

