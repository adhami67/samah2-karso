using System.Collections.Generic;
using IRIT.Framework.Resources;

namespace IRIT.Framework.Common.Enum
{
    public class GenderTypes
    {
        public static Dictionary<byte, string> GenderTypesDic => new Dictionary<byte, string>
        {
            {(byte) GenderTypesEnum.Male, GeneralResource.Male},
            {(byte) GenderTypesEnum.Female, GeneralResource.Female}
        };
    }

    public enum GenderTypesEnum
    {
        Male ,
        Female 
    }


}
