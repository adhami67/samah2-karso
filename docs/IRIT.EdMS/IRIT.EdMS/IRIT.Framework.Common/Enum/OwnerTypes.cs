using System.Collections.Generic;
using IRIT.Framework.Resources;

namespace IRIT.Framework.Common.Enum
{
    public class OwnerTypes
    {
        public static Dictionary<byte, string> OwnerTypeDic => new Dictionary<byte, string>
        {
            {(byte) OwnerTypesEnum.InfsRing, GeneralResource.InfraStructureRing},
            {(byte) OwnerTypesEnum.School, GeneralResource.School},
            {(byte) OwnerTypesEnum.SchoolGroup, GeneralResource.SchoolsGroup},
            {(byte) OwnerTypesEnum.Department, GeneralResource.Department}
        };
    }

    public enum OwnerTypesEnum
    {
        InfsRing,
        School,
        SchoolGroup,
        Department
    }
}
