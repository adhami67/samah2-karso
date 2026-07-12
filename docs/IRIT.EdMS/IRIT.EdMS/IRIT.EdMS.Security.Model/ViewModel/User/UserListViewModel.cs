namespace IRIT.EdMS.Security.Model.ViewModel.User
{
    public class UserListViewModel
    {
        public long Id { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public bool IsSystemAccount { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UsersGroups { get; set; }
    }
}
