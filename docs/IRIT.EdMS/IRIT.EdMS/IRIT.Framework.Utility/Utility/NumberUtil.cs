using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRIT.Framework.Utility.Utility
{
    public class NumberUtil
    {
        static NumberUtil()
        {
            PersianDigits = new string[10];
            PersianDigits[0] = "&#1632;";
            PersianDigits[1] = "&#1633;";
            PersianDigits[2] = "&#1634;";
            PersianDigits[3] = "&#1635;";
            PersianDigits[4] = "&#1636;";
            PersianDigits[5] = "&#1637;";
            PersianDigits[6] = "&#1638;";
            PersianDigits[7] = "&#1639;";
            PersianDigits[8] = "&#1640;";
            PersianDigits[9] = "&#1641;";
        }

        #region Members

        public static readonly string[] PersianDigits;

        #endregion

        #region Public Methods

        public static int GetRandom()
        {
            var objRandom = new Random((int)(DateTime.Now.Ticks % Int32.MaxValue));
            return objRandom.Next();
        }

        public static int GetRandom(int low, int high)
        {
            var objRandom = new Random((int)(DateTime.Now.Ticks % Int32.MaxValue));
            return objRandom.Next(low, high);
        }

        public static string ToPersian(int num)
        {
            var persianNumber = "";
            do
            {
                persianNumber = PersianDigits[num % 10] + persianNumber;
                num = num / 10;
            } while (num * 10 >= 10);
            return persianNumber;
        }

        public static string ToPersian(string value)
        {
            var pString = "";
            var chars = value.ToCharArray();
            foreach (var c in chars)
            {
                int num;
                if (int.TryParse(c.ToString(), out num))
                    pString = pString + PersianDigits[num];
                else
                    pString = pString + c;
            }

            return pString;
        }

        #endregion
    }
}
