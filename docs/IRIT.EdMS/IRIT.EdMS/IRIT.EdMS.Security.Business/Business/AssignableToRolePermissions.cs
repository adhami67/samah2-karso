using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using IRIT.EdMS.Security.Model.ViewModel;

namespace IRIT.EdMS.Security.Business.Business
{
    public static class AssignableToRolePermissions
    {
        #region Fields
        private static readonly Lazy<IEnumerable<PermissionViewModel>> PermissionsLazy =
            new Lazy<IEnumerable<PermissionViewModel>>(GetPermision, LazyThreadSafetyMode.ExecutionAndPublication);

        private static readonly Lazy<IEnumerable<string>> PermissionNamesLazy = new Lazy<IEnumerable<string>>(
            GetPermisionNames, LazyThreadSafetyMode.ExecutionAndPublication);
        #endregion

        #region permissionNames
        public static bool AllowSendPrivateMessage { get; set; }
        public static bool AllowSendNewsItem { get; set; }
        public static bool AllowSendBlogPostDraft { get; set; }
        public static bool AllowSendPollItem { get; set; }
        public static bool AllowSendFriendRequest { get; set; }
        public static bool CanUploadFile { get; set; }
        public static bool CanChangeProfilePicture { get; set; }
        public static bool CanModifyFirsAndLastName { get; set; }
        public const string CanEditRole = "CanEditRole";
        public const string CanDeleteRole = "CanDeleteRole";
        public const string CanViewRolesList = "CanViewRolesList";
        public const string CanCreateRole = "CanCreateRole";
        public const string CanEditUser = "CanEditUser";
        public const string CanCreateUser = "CanCreateUser";
        public const string CanDeleteUser = "CanDeleteUser";
        public const string CanSoftDeleteUser = "CanSoftDeleteUser";
        public const string CanViewUsersList = "CanViewUsersList";
        public const string CanEditUsersSetting = "CanEditUsersSetting";
        public const string CanAccessImages = "CanAccessImages";
        public const string CanViewAdminPanel = "CanViewAdminPanel";
        public const string CanAccessUsersFiles = "CanAccessUsersFiles";
        public const string CanAccessUsersAvatar = "CanAccessUsersAvatar";
        #endregion //permissions

        #region Permissions

        public static readonly PermissionViewModel CanEditRolePermission = new PermissionViewModel { Name = CanEditRole, Description = "میتوانند گروه کاربری را ویرایش کنند" };
        public static readonly PermissionViewModel CanDeleteRolePermission = new PermissionViewModel { Name = CanDeleteRole, Description = "میتوانند گروه کاربری را حذف کنند" };
        public static readonly PermissionViewModel CanViewRolesListPermission = new PermissionViewModel { Name = CanViewRolesList, Description = "میتوانند لیست گروه های کاربری را مشاهده کنند" };
        public static readonly PermissionViewModel CanCreateRolePermission = new PermissionViewModel { Name = CanCreateRole, Description = "میتوانند گروه کاربری جدید ایجاد کنند" };
        public static readonly PermissionViewModel CanEditUserPermission = new PermissionViewModel { Name = CanEditUser, Description = "میتوانند مشخصات کاربر را ویرایش کنند" };
        public static readonly PermissionViewModel CanCreateUserPermission = new PermissionViewModel { Name = CanCreateUser, Description = "میتوانند کاربر جدید ایجاد کنند" };
        public static readonly PermissionViewModel CanViewUsersListPermission = new PermissionViewModel { Name = CanViewUsersList, Description = "میتوانند لیست کاربران را مشاهده کنند" };
        public static readonly PermissionViewModel CanDeleteUserPermission = new PermissionViewModel { Name = CanDeleteUser, Description = "میتوانند کاربر را حذف کنند" };
        public static readonly PermissionViewModel CanSoftDeleteUserPermission = new PermissionViewModel { Name = CanSoftDeleteUser, Description = "میتوانند کاربر را به صورت منطقی حذف کنند" };
        public static readonly PermissionViewModel CanViewAdminPanelPermission = new PermissionViewModel { Name = CanViewAdminPanel, Description = "میتوانند پنل مدیریت را مشاهده کنند" };
        public static readonly PermissionViewModel CanEditUsersSettingPermission = new PermissionViewModel { Name = CanEditUsersSetting, Description = "میتوانند تنظیمات کاربران را ویرایش کنند" };
        public static readonly PermissionViewModel CanAccessImagesPermission = new PermissionViewModel { Name = CanAccessImages, Description = "میتوانند به تصاویر دسترسی داشته باشند" };
        public static readonly PermissionViewModel CanAccessUsersAvatarPermission = new PermissionViewModel { Name = CanAccessUsersAvatar, Description = "میتوانند به تصاویر پروفایل کاربرن دسترسی داشته باشند" };
        public static readonly PermissionViewModel CanAccessUsersFilesPermission = new PermissionViewModel { Name = CanAccessUsersFiles, Description = "میتوانند فایل های ضمیمه شده توسط کاربران را دانلود کنند" };
        #endregion

        #region Properties
        public static IEnumerable<PermissionViewModel> Permissions
        {
            get
            {
                return PermissionsLazy.Value;
            }
        }

        public static IEnumerable<string> PermissionNames
        {
            get
            {
                return PermissionNamesLazy.Value;
            }
        }

        #endregion

        #region GetAllPermisions
        private static IEnumerable<PermissionViewModel> GetPermision()
        {
            return new List<PermissionViewModel>
            {
                CanAccessImagesPermission,
                CanAccessUsersAvatarPermission,
                CanAccessUsersFilesPermission,
                CanCreateRolePermission,
                CanCreateUserPermission,
                CanDeleteRolePermission,
                CanDeleteUserPermission,
                CanEditRolePermission,
                CanEditUserPermission,
                CanEditUsersSettingPermission,
                CanSoftDeleteUserPermission,
                CanViewAdminPanelPermission,
                CanViewRolesListPermission,
                CanViewUsersListPermission
            };
        }

        private static IEnumerable<string> GetPermisionNames()
        {
            return new List<string>()
            {
               CanEditRole ,
               CanDeleteRole,
               CanViewRolesList ,
               CanCreateRole ,
               CanEditUser ,
               CanCreateUser ,
               CanDeleteUser,
               CanSoftDeleteUser ,
               CanViewUsersList ,
               CanEditUsersSetting ,
               CanAccessImages ,
               CanViewAdminPanel ,
               CanAccessUsersFiles ,
               CanAccessUsersAvatar
            };
        }
        #endregion

        #region GetAsSelectedList

        public static IEnumerable<SelectListItem> GetAsSelectListItems()
        {
            return Permissions.Select(a => new SelectListItem { Text = a.Description, Value = a.Name }).ToList();
        }
        #endregion
    }
}
