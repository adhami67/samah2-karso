using System.Collections.Generic;
using IRIT.Framework.Resources;

namespace IRIT.Framework.Common.Enum
{
    public class EmploymentTypes
    {
        public static Dictionary<byte, string> ActionKindDic => new Dictionary<byte, string>
        {
            {(byte) EmploymentTypesEnum.Official, GeneralResource.Official},
            {(byte) EmploymentTypesEnum.Covenant, GeneralResource.Covenant},
            {(byte) EmploymentTypesEnum.Contract, GeneralResource.Contract}
        };
    }

    public enum EmploymentTypesEnum
    {

        Official,
        Covenant,
        Contract
    }
}
