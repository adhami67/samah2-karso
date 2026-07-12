using System.Security.Principal;

namespace IRIT.Framework.Utility.Security
{
    interface IEdMSPrincipal : IPrincipal
    {
        long Id { get; set; }

        long PartyId { get; set; }

        long SchoolId { get; set; }

        string FullName { get; set; }

        string UserName { get; set; }

        string UserGroupIds { get; set; }
    }
}
