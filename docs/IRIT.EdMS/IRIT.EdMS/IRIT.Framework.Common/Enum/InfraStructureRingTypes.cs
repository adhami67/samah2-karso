using System.Collections.Generic;
using IRIT.Framework.Resources;

namespace IRIT.Framework.Common.Enum
{
 

    public class InfraStructureRingTypes
    {
        public static Dictionary<byte, string> InfraStructureRingTypesDic => new Dictionary<byte, string>
        {
            {(byte) InfraStructureRingTypesEnum.Government, GeneralResource.Government},
            {(byte) InfraStructureRingTypesEnum.Private, GeneralResource.Private},
            {(byte) InfraStructureRingTypesEnum.SemiGovernment, GeneralResource.SemiGovernment},
            {(byte) InfraStructureRingTypesEnum.BoardOfTrustees, GeneralResource.BoardOfTrustees}
        };
    }

    public enum InfraStructureRingTypesEnum
    {
        Government,
        Private,
        SemiGovernment,
        BoardOfTrustees

    }
}
