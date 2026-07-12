using System.Web.Mvc;
using AutoMapper;
using IRIT.EdMS.Security.Model.ViewModel.UserGroup;
using IRIT.Framework.DataModel.Security;
using IRIT.Framework.Utility.AutoMapper.Extentions;

namespace IRIT.EdMS.Security.Business.AutoMapperProfiles
{
    public class UserGroupProfile : Profile
    {
        public override string ProfileName => GetType().Name;

        protected override void Configure()
        {
            CreateMap<UsersGroup, UsersGroupViewModel>().IgnoreAllNonExisting();

            CreateMap<UsersGroup, SelectListItem>()
                .ForMember(d => d.Text, m => m.MapFrom(s => s.GroupName))
                .ForMember(d => d.Value, m => m.MapFrom(s => s.Id));
        }        
    }
}
