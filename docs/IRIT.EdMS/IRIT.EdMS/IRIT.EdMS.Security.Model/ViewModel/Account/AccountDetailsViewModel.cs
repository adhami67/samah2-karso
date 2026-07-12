using IRIT.Framework.Common.Enum;

namespace IRIT.EdMS.Security.Model.ViewModel.Account
{
    public class AccountDetailsViewModel
    {
        public long UserId { get; set; }

        public long PartyId { get; set; }

        public LoginStatus Status { get; set; }

        public long[] UserGroupIds { get; set; }

        public long? SchoolId { get; set; }

        public string FullName { get; set; }

        public byte[] SmallImage { get; set; }
    }
}
