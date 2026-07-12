using System.Collections.Generic;
using IRIT.Framework.Resources;

namespace IRIT.Framework.Common.Enum
{
    public class RegionTypes
    {
        public static Dictionary<byte, string> RegionKindDic => new Dictionary<byte, string>
        {
            {(byte) RegionKindEnum.Country, GeneralResource.Country},
            {(byte) RegionKindEnum.Province, GeneralResource.Province},
            {(byte) RegionKindEnum.City, GeneralResource.City},
        };
    }

    public enum RegionKindEnum
    {
        Country,
        Province,
        City
    }
}
