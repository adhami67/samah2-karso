using AutoMapper;
using IRIT.EdMS.Security.Model.ViewModel.User;
using IRIT.Framework.DataModel.Security;
using IRIT.Framework.Utility.AutoMapper.Extentions;

namespace IRIT.EdMS.Security.Business.AutoMapperProfiles
{
    public class UserProfile : Profile
    {
        public override string ProfileName => GetType().Name;

        protected override void Configure()
        {
            CreateMap<User, UserListViewModel>()
                .ForMember(d => d.UsersGroups, m => m.Ignore()).IgnoreAllNonExisting();

            //CreateMap<UserViewModel, User>()
            //    .ForMember(d => d.RegisterDate, m => m.MapFrom(s => DateTime.Now))
            //    .ForMember(d => d.LastActivityDate, m => m.MapFrom(s => DateTime.Now))
            //    .ForMember(d => d., m => m.MapFrom(s => s.Email.FixGmailDots()))
            //    .ForMember(d => d.UserName, m => m.MapFrom(s => s.UserName.ToLower()))
            //    .IgnoreAllNonExisting();

            //CreateMap<EditUserViewModel, User>()
            //    .ForMember(d => d.Roles, m => m.MapFrom(s => new Collection<UserRole>()))
            //    .ForMember(d => d.RegisterDate, m => m.Ignore())
            //    .ForMember(d => d.LastActivityDate, m => m.Ignore())
            //    .ForMember(d => d.BirthDay, m => m.Ignore())
            //    .ForMember(d => d.EmailConfirmed, m => m.Ignore())
            //    .ForMember(d => d.Email, m => m.Ignore())
            //    .ForMember(d => d.UserName, m => m.MapFrom(s => s.UserName.ToLower()))
            //     .IgnoreAllNonExisting();

            //CreateMap<User, EditUserViewModel>().IgnoreAllNonExisting();

            //CreateMap<RegisterViewModel, User>()
            //    .ForMember(d => d.RegisterDate, a => a.MapFrom(s => DateTime.Now))
            //    .ForMember(d => d.LastActivityDate, m => m.MapFrom(s => DateTime.Now))
            //    .ForMember(d => d.AvatarFileName, a => a.MapFrom(s => "avatar.jpg"))
            //    .ForMember(d => d.Email, m => m.MapFrom(s => s.Email.FixGmailDots()))
            //    .ForMember(d => d.UserName, m => m.MapFrom(s => s.UserName.ToLower()))
            //    .IgnoreAllNonExisting();
        }        
    }
}
