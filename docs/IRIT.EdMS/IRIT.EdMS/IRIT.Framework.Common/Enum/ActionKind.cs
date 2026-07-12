using System.Collections.Generic;
using IRIT.Framework.Resources;

namespace IRIT.Framework.Common.Enum
{
    public class ActionKind
    {
        public static Dictionary<byte, string> ActionKindDic => new Dictionary<byte, string>
        {
            
            {(byte) ActionKindEnum.Insert, GeneralResource.Insert},
            {(byte) ActionKindEnum.Update, GeneralResource.Update},
            {(byte) ActionKindEnum.Delete, GeneralResource.Delete},
            {(byte) ActionKindEnum.View, GeneralResource.View}

        };
    }

    public enum ActionKindEnum
    {
    
        Insert,
        Update,
        Delete,
        View
    }
}
