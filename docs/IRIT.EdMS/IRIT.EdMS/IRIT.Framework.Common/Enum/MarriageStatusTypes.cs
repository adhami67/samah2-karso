using System.Collections.Generic;
using IRIT.Framework.Resources;

namespace IRIT.Framework.Common.Enum
{
    public class MarriageStatusTypes
    {
        public static Dictionary<byte, string> MarriageStatusTypesDic => new Dictionary<byte, string>
        {
            {(byte) MarriageStatusTypesEnum.Married, GeneralResource.Married},
            {(byte) MarriageStatusTypesEnum.Single, GeneralResource.Single},
            {(byte) MarriageStatusTypesEnum.Divorced, GeneralResource.Divorced}
        };
    }

    public enum MarriageStatusTypesEnum
    {
        Married,
        Single,
        Divorced
    }
}
