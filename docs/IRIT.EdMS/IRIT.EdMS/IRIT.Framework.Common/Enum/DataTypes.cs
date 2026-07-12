using System.Collections.Generic;
using IRIT.Framework.Resources;

namespace IRIT.Framework.Common.Enum
{
    public class DataTypes
    {
        public static Dictionary<int, string> DataTypesDic => new Dictionary<int, string>
        {
            {(int) DataTypesEnum.Int, GeneralResource.Person},
            {(int) DataTypesEnum.Float, GeneralResource.Company},
            {(int) DataTypesEnum.Decimal, GeneralResource.Customer},
            {(int) DataTypesEnum.String, GeneralResource.Employee},
            {(int) DataTypesEnum.Date, GeneralResource.Supplier},
            {(int) DataTypesEnum.Bool, GeneralResource.Parents},
            {(int) DataTypesEnum.LookupKind, GeneralResource.Student}
        };
    }

    public enum DataTypesEnum
    {
        Int = 0,
        Float ,
        Decimal,
        String,
        Date ,
        Char ,
        Bool,
        LookupKind 
    }
}
