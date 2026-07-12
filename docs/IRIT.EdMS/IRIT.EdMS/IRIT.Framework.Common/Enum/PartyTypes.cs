using System;
using System.Collections.Generic;
using IRIT.Framework.Resources;

namespace IRIT.Framework.Common.Enum
{
    public class PartyTypes
    {
        public static Dictionary<int, string> PartyTypeDic => new Dictionary<int, string>
        {
            {(int) PartyTypesEnum.Person, GeneralResource.Person},
            {(int) PartyTypesEnum.Company, GeneralResource.Company},
            {(int) PartyTypesEnum.Customer, GeneralResource.Customer},
            {(int) PartyTypesEnum.Employee, GeneralResource.Employee},
            {(int) PartyTypesEnum.Supplier, GeneralResource.Supplier},
            {(int) PartyTypesEnum.Parents, GeneralResource.Parents},
            {(int) PartyTypesEnum.Student, GeneralResource.Student}
        };
    }

   [Flags]
   public enum PartyTypesEnum
    {
        Person = 0,
        Supplier = 1,
        Customer = 2,
        Student = 4,
        Employee = 8,
        Company = 16,
        Parents = 32
    }
}
