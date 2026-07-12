using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IRIT.EdMS.Common.Business;
using IRIT.EdMS.Common.Business.Business;
using IRIT.EdMS.Common.Business.Contract;
using IRIT.Framework.DataAccess.Context;
using IRIT.Framework.Utility.Controller;

namespace IRIT.EdMS.WebUI.Areas.Common.Controllers
{
    [Authorize]
    public partial class PartyOwnerController : BaseController
    {
        #region Data Member
        private ApplicationDbContext _dbContext { get; }
        private readonly IPartyOwner _partyOwnerBl;
        private readonly ISchool _schoolBl;
        #endregion

        #region Constractor
        public PartyOwnerController()
        {
            _dbContext = new ApplicationDbContext();
            _partyOwnerBl = new PartyOwnerBl(_dbContext);
            _schoolBl = new SchoolBl(_dbContext);
        }
        #endregion

        #region Get
        public virtual ActionResult Index()
        {
            FillSelectList();
            return View();
        }
        #endregion

        #region private
        private void FillSelectList()
        {
            #region PartyOwner
            ViewBag.PartyOwners = new SelectList(_partyOwnerBl.GetSchoolOwner(User.Id, User.PartyId), "Id", "OwnerTitle");
            #endregion
        }
        #endregion
    }
}