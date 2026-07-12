using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRIT.Framework.Resources;

namespace IRIT.Framework.Common.Enum
{
    public class MilitaryServiceTypes
    {
        public static Dictionary<byte, string> MilitaryServiceTypesDic => new Dictionary<byte, string>
        {
            { (byte) MilitaryServiceTypesEnum.MilitaryServiceDoned, GeneralResource.MilitaryServiceDoned },
            { (byte) MilitaryServiceTypesEnum.MilitaryServiceExemption, GeneralResource.MilitaryServiceExemption },
            { (byte) MilitaryServiceTypesEnum.MilitaryServiceUnDone, GeneralResource.MilitaryServiceUnDone }
        };
    }

    public enum MilitaryServiceTypesEnum
    {
        MilitaryServiceDoned,
        MilitaryServiceUnDone,
        MilitaryServiceExemption

    }
}
