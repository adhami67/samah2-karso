using System.Collections.Generic;
using IRIT.Framework.Resources;

namespace IRIT.Framework.Common.Enum
{
    public class RenterTypes
    {
        public static Dictionary<byte, string> RenterKindDic => new Dictionary<byte, string>
        {
            {(byte) RenterKindEnum.Temporary, GeneralResource.Temporary},
            {(byte) RenterKindEnum.Permanent, GeneralResource.Permanent}
        };
    }

    public enum RenterKindEnum
    {
        Temporary,
        Permanent
    }
}
