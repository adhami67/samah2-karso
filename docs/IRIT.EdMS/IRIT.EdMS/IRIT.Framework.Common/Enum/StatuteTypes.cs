using System.Collections.Generic;
using IRIT.Framework.Resources;

namespace IRIT.Framework.Common.Enum
{
    public class StatuteTypes
    {
        public static Dictionary<byte, string> StatuteTypesDic => new Dictionary<byte, string>
        {
            {(byte) StatuteTypesEnum.Assignment, GeneralResource.Assignment},
            {(byte) StatuteTypesEnum.Deposal, GeneralResource.Deposal},
            {(byte) StatuteTypesEnum.Abdication, GeneralResource.Abdication},
            {(byte) StatuteTypesEnum.FinishWork, GeneralResource.FinishWork}
        };
    }

    public enum StatuteTypesEnum
    {
        Assignment,
        Deposal,
        Abdication,
        FinishWork
    }
}
