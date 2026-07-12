#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using IRIT.EdMS.Common.Business;
using IRIT.EdMS.Common.Business.Business;
using IRIT.EdMS.Common.Business.Contract;
using IRIT.EdMS.HumanResource.Model.ViewModel.EmployeeStatute;
using IRIT.EdMS.HumanResource.Model.ViewModel.OrganizationStructure;
using IRIT.EdMS.HumanResources.Business.Business;
using IRIT.EdMS.HumanResources.Business.Contracts;
using IRIT.EdMS.Security.Business;
using IRIT.EdMS.Security.Contracts;
using IRIT.Framework.Common.Enum;
using IRIT.Framework.DataAccess.Context;
using IRIT.Framework.DataModel;
using IRIT.Framework.DataModel.HumanResources;
using IRIT.Framework.Resources;
using IRIT.Framework.Utility.Controller;
using IRIT.Framework.Utility.Filters;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
#endregion

namespace IRIT.EdMS.WebUI.Areas.HumanResource.Controllers
{
    [Authorize]
    public partial class OrganizationStructureController : BaseController
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
        public OrganizationStructureController()
        {
            _DbContext = new ApplicationDbContext();
            _scoolBl = new SchoolBl(_DbContext);
            _organizationStructureBl = new OrganizationStructureBl(_DbContext);
            _organizationStructures = new List<OrganizationStructure>();
            _employeeStatuteBl = new EmployeeStatuteBl(_DbContext);
            _employeeBl = new EmployeeBl(_DbContext);
        }
        #endregion

        #region Organization Tree

        #region Get
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

        #endregion

        #region Tree Management
        public virtual ActionResult ManageOrganization(string hiddenNodeId)
        {
            var status = hiddenNodeId.Split('|');
            switch (status[0])
            {
                case "AddOrganization":
                    return AddOrganiz(status[1]);

                case "AddRole":
                    return AddRole(status[1]);

                case "Edit":
                    return EditNode(status[1]);

                case "Delete":
                    return View(status[1].StartsWith("1") ? "EditOrganization" : "EditRole");

                default:
                    this.NotyError(GeneralResource.ErrorInRequest, true);
                    return RedirectToAction(MVC.HumanResource.OrganizationStructure.ActionNames.Index);
            }
        }

        #endregion

        #region Create

        #region Organization

        private ActionResult AddOrganiz(string status)
        {
            var parentId = status.Split('_')[1] != "0" ? long.Parse(status.Split('_')[1]) : (long?)null;
            var organization = new OrganizationStructureViewModel { ParentId = parentId, DatePicker = DateTime.Now };
            return View("CreateOrganization", organization);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[IRITAuthorize(AssignableToRolePermissions.CanCreateUser)]
        public virtual ActionResult CreateOrganization(OrganizationStructureViewModel viewModel)
        {
            _organizationStructureBl.Add(new OrganizationStructure
            {
                Code = viewModel.Code,
                Comment = viewModel.Comment,
                CreatorId = User.Id,
                IsDeleted = false,
                Title = viewModel.Title,
                SchoolId = User.SchoolId,
                StructureType = StructureTypesEnum.OrganizationStructure,
                CreateTime = DateTime.Now,
                ParentId = viewModel.ParentId,
                ActivationDate = viewModel.DatePicker.Date
            });
            _DbContext.SaveChanges();
            this.NotySuccess(GeneralResource.Add_successful);
            return RedirectToAction(MVC.HumanResource.OrganizationStructure.ActionNames.Index, MVC.HumanResource.OrganizationStructure.Name);
        }
        #endregion

        #region Role
        private ActionResult AddRole(string status)
        {
            var parentId = status.Split('_')[1] != "0" ? long.Parse(status.Split('_')[1]) : (long?)null;
            var roleViewModel = new RoleStructureViewModel() { ParentId = parentId, DatePicker = DateTime.Now };
            return View("CreateRole", roleViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult CreateRole(RoleStructureViewModel viewModel)
        {
            _organizationStructureBl.Add(new OrganizationStructure
            {
                Code = viewModel.Code,
                Comment = viewModel.Comment,
                CreatorId = User.Id,
                IsDeleted = false,
                Capacity = viewModel.Capacity,
                Title = viewModel.Title,
                SchoolId = User.SchoolId,
                StructureType = StructureTypesEnum.RoleStructure,
                CreateTime = DateTime.Now,
                ParentId = viewModel.ParentId,
                ActivationDate = viewModel.DatePicker.Date
            });
            _DbContext.SaveChanges();
            this.NotySuccess(GeneralResource.Add_successful);
            return RedirectToAction(MVC.HumanResource.OrganizationStructure.ActionNames.Index, MVC.HumanResource.OrganizationStructure.Name);
        }
        #endregion

        #endregion

        #region Edit
        private ActionResult EditNode(string status)
        {
            var statusItem = status.Split('_');
            var org = _organizationStructureBl.GetById(long.Parse(statusItem[1]));
            if (statusItem[0] == "1")
            {
                var organization = new OrganizationStructureViewModel { Id = org.Id, DatePicker = org.ActivationDate, Title = org.Title, Code = org.Code, Comment = org.Comment, ParentId = org.ParentId };
                return View("EditOrganization", organization);
            }
            else
            {
                var roleOrganization = new RoleStructureViewModel { Id = org.Id, Capacity = org.Capacity, DatePicker = org.ActivationDate, Title = org.Title, Code = org.Code, Comment = org.Comment, ParentId = org.ParentId };
                return View("EditRole", roleOrganization);
            }
        }

        [HttpPost]
        //[Route("Edit/{id}")]
        [ValidateAntiForgeryToken]
        //[IRITAuthorize(AssignableToRolePermissions.CanEditUser)]
        public virtual ActionResult EditOrganization(OrganizationStructureViewModel viewModel)
        {
            var organiz = _organizationStructureBl.GetById(viewModel.Id);
            organiz.Code = viewModel.Code;
            organiz.Title = viewModel.Title;
            organiz.ActivationDate = viewModel.DatePicker.Date;
            organiz.Comment = viewModel.Comment;

            _organizationStructureBl.Edit(organiz);
            _DbContext.SaveAllChanges();

            this.NotySuccess(GeneralResource.Edit_successful);
            return RedirectToAction(MVC.HumanResource.OrganizationStructure.ActionNames.Index, MVC.HumanResource.OrganizationStructure.Name);
        }

        [HttpPost]
        //[Route("Edit/{id}")]
        [ValidateAntiForgeryToken]
        //[IRITAuthorize(AssignableToRolePermissions.CanEditUser)]
        public virtual ActionResult EditRole(RoleStructureViewModel viewModel)
        {
            var roleOrganiz = _organizationStructureBl.GetById(viewModel.Id);
            roleOrganiz.Code = viewModel.Code;
            roleOrganiz.Title = viewModel.Title;
            roleOrganiz.ActivationDate = viewModel.DatePicker.Date;
            roleOrganiz.Comment = viewModel.Comment;
            roleOrganiz.Capacity = viewModel.Capacity;

            _organizationStructureBl.Edit(roleOrganiz);

            _DbContext.SaveAllChanges();
            this.NotySuccess(GeneralResource.Edit_successful);
            return RedirectToAction(MVC.HumanResource.OrganizationStructure.ActionNames.Index, MVC.HumanResource.OrganizationStructure.Name);
        }
        #endregion

        #region Delete
        public virtual JsonResult Delete(string id)
        {
            var orgId = long.Parse(id.Split('_')[1]);
            var org = _organizationStructureBl.GetById(orgId);
            org.IsDeleted = true;
            _organizationStructureBl.Edit(org);
            _DbContext.SaveChanges();

            var strMessage = string.Empty;
            //strMessage = "شما قادر به حذف این رکورد نمی باشید";
            return Json(new { Message = strMessage }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region EmployeeStatute Grid
        #region Get
        public virtual ActionResult EmployeeStatute_Read([DataSourceRequest]DataSourceRequest request, string specialityIn)
        {
            long orgId;
            long.TryParse(specialityIn, out orgId);

            if (orgId == 0)
            {
                return Json(new List<EmployeeStatute>().ToDataSourceResult(request));
            }

            var employees = _employeeStatuteBl.Get(c => c.OrganizationStructureId == orgId);
            return Json(employees.ToDataSourceResult(request));
        }
        #endregion

        #region Manage
        public virtual ActionResult ManageEmployeeStatute(string hiddenOrgId)
        {
            var status = hiddenOrgId.Split('|');
            switch (status[0])
            {
                case "Add":
                    return AddEmployeeStatute(status[1]);

                default:
                    this.NotyError(GeneralResource.ErrorInRequest, true);
                    return RedirectToAction(MVC.HumanResource.OrganizationStructure.ActionNames.Index);
            }
        }
        #endregion

        #region Create
        private ActionResult AddEmployeeStatute(string status)
        {
            var roleId = long.Parse(status.Split('_')[1]);
            var roleInfo = _organizationStructureBl.GetById(roleId);
            var empForRol = _employeeStatuteBl.Get(c => c.OrganizationRoleId == roleInfo.Id).Select(c => c.EmployeeId).ToList();

            if (empForRol.Count >= roleInfo.Capacity)
            {
                this.NotyError(GeneralResource.FullCapacity, true);
                RedirectToAction(MVC.HumanResource.OrganizationStructure.ActionNames.Index);
            }

            ViewBag.Employees =
                new SelectList(
                    _employeeBl.Get(c => !empForRol.Contains(c.Id))
                        .Select(c => new {c.Id, FullName = $"{c.Party.FirstName} {c.Party.LastName}" })
                        .ToList(), "ID", "FullName");

            var empStatut = new EmployeeStatuteViewModel
            {
                OrganizationStructureId = roleInfo.Id,
                ApplyDate = DateTime.Now
            };

            return View("CreateEmployeeStatute", empStatut);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[IRITAuthorize(AssignableToRolePermissions.CanCreateUser)]
        public virtual ActionResult CreateEmployeeStatute(EmployeeStatuteViewModel viewModel)
        {
            _employeeStatuteBl.Add(new EmployeeStatute
            {
                Comment = viewModel.Comment,
                CreatorId = User.Id,
                IsDeleted = false,
                CreateTime = DateTime.Now,
                ApplyDate = viewModel.ApplyDate.Date,
                EmployeeId = viewModel.EmployeeId.Value,
                IssueDate = DateTime.Now,
                OrganizationRoleId = viewModel.OrganizationStructureId,
                OrganizationStructureId = viewModel.OrganizationStructureId,
                StatuteType = StatuteTypesEnum.Assignment
            });
            _DbContext.SaveChanges();

            this.NotySuccess(GeneralResource.Add_successful);
            return RedirectToAction(MVC.HumanResource.OrganizationStructure.ActionNames.Index, MVC.HumanResource.OrganizationStructure.Name);
        }
        #endregion

        #region Edit
        //[Route("Edit/{id}")]
        [HttpGet]
        //[IRITAuthorize(AssignableToRolePermissions.CanEditUser)]
        public virtual ActionResult EditEmployeeStatute(long id)
        {
            var empStatute = _employeeStatuteBl.GetById(id);
            var empstViewModel = new EmployeeStatuteViewModel {Id = empStatute.Id, ApplyDate = empStatute.ApplyDate , Comment = empStatute.Comment };
            return View(empstViewModel);
        }

        [HttpPost]
        //[Route("Edit/{id}")]
        [ValidateAntiForgeryToken]
        //[IRITAuthorize(AssignableToRolePermissions.CanEditUser)]
        public virtual ActionResult EditEmployeeStatute(EmployeeStatuteViewModel viewModel)
        {
            var empStatute = _employeeStatuteBl.GetById(viewModel.Id);
            empStatute.ApplyDate = viewModel.ApplyDate.Date;
            empStatute.Comment = viewModel.Comment;
            _employeeStatuteBl.Edit(empStatute);
            _DbContext.SaveChanges();

            this.NotySuccess(GeneralResource.Edit_successful);
            return RedirectToAction(MVC.HumanResource.OrganizationStructure.ActionNames.Index, MVC.HumanResource.OrganizationStructure.Name);
        }
        #endregion

        #region Delete
        public virtual JsonResult DeleteEmployeeStatute(string id)
        {
            var empStatute = _employeeStatuteBl.GetById(long.Parse(id));
            empStatute.ExpiryDate = DateTime.Now;
            empStatute.IsDeleted = true;
            _employeeStatuteBl.Edit(empStatute);
            _DbContext.SaveChanges();

            var strMessage = string.Empty;

            //strMessage = "شما قادر به حذف این رکورد نمی باشید";
            return Json(new { Message = strMessage }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region Private
        private void FillSelectList()
        {

        }
        #endregion
    }
}