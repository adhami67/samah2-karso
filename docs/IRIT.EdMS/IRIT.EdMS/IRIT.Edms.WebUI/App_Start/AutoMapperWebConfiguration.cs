using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IRIT.EdMS.WebUI
{
    public static class AutoMapperWebConfiguration
    {
        public static void Configure()
        {
            Security.Business.AutoMapperProfiles.AutoMapperConfiguration.Configure();
        }
    }
}