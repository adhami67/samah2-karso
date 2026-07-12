using System.Collections.Generic;
using IRIT.Framework.Resources;

namespace IRIT.Framework.Common.Enum
{
    public class ResourceKind
    {
        public static Dictionary<byte, string> ResourceKindDic => new Dictionary<byte, string>
        {
            {(byte) ResourceKindEnum.SubSystem, GeneralResource.SubSystem},
            {(byte) ResourceKindEnum.Module, GeneralResource.Module},
            {(byte) ResourceKindEnum.Entity, GeneralResource.Entity}
        };
    }

    public enum ResourceKindEnum
    {
        SubSystem,
        Module,
        Entity
    }
}
