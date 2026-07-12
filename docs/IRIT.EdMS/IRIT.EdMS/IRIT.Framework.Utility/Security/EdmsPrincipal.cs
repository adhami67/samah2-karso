using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace IRIT.Framework.Utility.Security
{
    public class EdmsPrincipal : IEdMSPrincipal
    {
        public IIdentity Identity => new GenericIdentity(UserName);

        public bool IsInRole(string role) { return false; }

        public long Id { get; set; }

        public long PartyId { get; set; }

        public long SchoolId { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public byte[] UserSmallImage { get; set; }

        public string UserGroupIds { get; set; }
    }
}
