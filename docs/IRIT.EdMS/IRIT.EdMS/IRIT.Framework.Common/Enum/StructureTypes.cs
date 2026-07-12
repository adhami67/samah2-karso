using System.Collections.Generic;
using IRIT.Framework.Resources;

namespace IRIT.Framework.Common.Enum
{
    public class StructureTypes
    {
        public static Dictionary<byte, string> StructureTypesDic => new Dictionary<byte, string>
        {
            {(byte) StructureTypesEnum.OrganizationStructure, GeneralResource.OrganizationStructure},
            {(byte) StructureTypesEnum.RoleStructure, GeneralResource.RoleStructure}
        };
    }

    public enum StructureTypesEnum
    {
        OrganizationStructure,
        RoleStructure 
    }
}
