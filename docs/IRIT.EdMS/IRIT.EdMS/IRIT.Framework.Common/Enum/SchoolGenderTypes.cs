using System.Collections.Generic;
using IRIT.Framework.Resources;

namespace IRIT.Framework.Common.Enum
{
    public class SchoolGenderTypes
    {
        public static Dictionary<byte, string> SchoolGenderTypeDic => new Dictionary<byte, string>
        {
            {(byte) SchoolGenderTypesEnum.Boys, GeneralResource.Boys},
            {(byte) SchoolGenderTypesEnum.Girly, GeneralResource.Girly},
            {(byte) SchoolGenderTypesEnum.Complex, GeneralResource.Complex}
        };
    }

    public enum SchoolGenderTypesEnum
    {
        Boys,
        Girly,
        Complex
    }
}
