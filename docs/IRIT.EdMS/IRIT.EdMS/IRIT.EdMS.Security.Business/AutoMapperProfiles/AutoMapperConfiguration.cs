namespace IRIT.EdMS.Security.Business.AutoMapperProfiles
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            AutoMapper.Mapper.Initialize(c=> c.AddProfile(new UserGroupProfile()));

            //AutoMapper.Mapper.Initialize(c => c.AddProfile(new UserProfile()));
        }
    }
}
