using System.Collections.Generic;
using IRIT.Framework.Resources;

namespace IRIT.Framework.Common.Enum
{
    public class AccessKind
    {
        public static Dictionary<byte, string> AccessKindDic => new Dictionary<byte, string>
        {
            {(byte) AccessKindEnum.Grant, GeneralResource.Grant},
            {(byte) AccessKindEnum.Deny, GeneralResource.Deny}
        };
    }

    public enum AccessKindEnum
    {
        Grant,
        Deny
    }
}
