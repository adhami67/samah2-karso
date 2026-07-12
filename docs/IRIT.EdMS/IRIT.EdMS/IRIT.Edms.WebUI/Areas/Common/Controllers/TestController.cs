using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IRIT.EdMS.Common.Business;
using IRIT.EdMS.Common.Business.Business;
using IRIT.EdMS.Common.Business.Contract;
using IRIT.EdMS.HumanResources.Business.Business;
using IRIT.EdMS.HumanResources.Business.Contracts;
using IRIT.Framework.Common.Enum;
using IRIT.Framework.DataAccess.Context;
using IRIT.Framework.DataModel;
using IRIT.Framework.DataModel.HumanResources;
using IRIT.Framework.Utility.Controller;
using Kendo.Mvc.UI;

namespace IRIT.EdMS.WebUI.Areas.Common.Controllers
{
    [Authorize]
    public partial class TestController : BaseController
    {
        #region Data Member
        public ApplicationDbContext _DbContext { get; }
        private readonly IOrganizationStructure _organizationStructureBl;
        private readonly ISchool _scoolBl;
        private readonly IEmployee _employeeBl;
        private readonly IEmployeeStatute _employeeStatuteBl;
        private List<OrganizationStructure> _organizationStructures;
        #endregion

        #region Constractor
        public TestController()
        {
            _DbContext = new ApplicationDbContext();
            _scoolBl = new SchoolBl(_DbContext);
            _organizationStructureBl = new OrganizationStructureBl(_DbContext);
            _organizationStructures = new List<OrganizationStructure>();
            _employeeStatuteBl = new EmployeeStatuteBl(_DbContext);
            _employeeBl = new EmployeeBl(_DbContext);
        }
        #endregion

        public virtual ActionResult Index()
        {
            ViewBag.OrganizationStructureData = GetDefaultInlineData();
            return View();
        }

        private IEnumerable<TreeViewItemModel> GetDefaultInlineData()
        {
            var school = _scoolBl.GetById(User.SchoolId);

            _organizationStructures = _organizationStructureBl.Get(c => c.SchoolId == school.Id);

            var treeNodes = PopulateTreeView(null);
            return new List<TreeViewItemModel> { new TreeViewItemModel { Text = school.SchoolName, Id = "0_0", Items = treeNodes, ImageUrl = "~/Content/Image/Icon/Organiz.png" } };
        }

        private List<TreeViewItemModel> PopulateTreeView(long? parentId)
        {
            var nodes = new List<TreeViewItemModel>();
            foreach (var source in _organizationStructures.Where(c => c.ParentId == parentId))
            {
                var structureKindId = source.StructureType == StructureTypesEnum.OrganizationStructure ? "1" : "2";
                var imageUrl = source.StructureType == StructureTypesEnum.OrganizationStructure ? "~/Content/Image/Icon/Squer.png" : "~/Content/Image/Icon/UserRole.png";
                var node = new TreeViewItemModel() { Text = source.Title, Id = $"{structureKindId}_{source.Id}", ImageUrl = imageUrl };
                if (_organizationStructures.Any(c => c.ParentId == source.Id))
                {
                    node.Items = PopulateTreeView(source.Id);
                }

                nodes.Add(node);
            }
            return nodes;
        }
    }
}