using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRIT.Framework.Utility.Security
{
    public class EdMSPrincipalSerializeModel
    {
        public long Id { get; set; }

        public long PartyId { get; set; }

        public long? SchoolId { get; set; }

        public string FullName { get; set; }

        public string UserName { get; set; }

        public string UserGroupIds { get; set; }

        public byte[] UserSmallImage { get; set; }
    }
}
