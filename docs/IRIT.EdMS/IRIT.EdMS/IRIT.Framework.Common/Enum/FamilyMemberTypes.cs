using System.Collections.Generic;
using IRIT.Framework.Resources;

namespace IRIT.Framework.Common.Enum
{
    public class FamilyMemberTypes
    {
        public static Dictionary<byte, string> FamilyMemberKindDic => new Dictionary<byte, string>
        {
            {(byte) FamilyMemberKindEnum.Father, GeneralResource.Father},
            {(byte) FamilyMemberKindEnum.Mother, GeneralResource.Mother},
            {(byte) FamilyMemberKindEnum.Brother, GeneralResource.Brother},
            {(byte) FamilyMemberKindEnum.Sister, GeneralResource.Sister},
            {(byte) FamilyMemberKindEnum.Spouse, GeneralResource.Spouse},
            {(byte) FamilyMemberKindEnum.Child, GeneralResource.Child},
            {(byte) FamilyMemberKindEnum.Others, GeneralResource.Others},
        };
    }

    public enum FamilyMemberKindEnum
    {
        Father,
        Mother,
        Brother,
        Sister,
        Spouse,
        Child,
        Others,

    }
}
