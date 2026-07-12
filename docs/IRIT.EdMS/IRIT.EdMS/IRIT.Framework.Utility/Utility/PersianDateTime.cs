using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRIT.Framework.Utility.Utility
{
    public class PersianDateTime
    {
        #region Members

        private DateTime _eDateTime;
        private int _hour;
        private int _minute;
        private int _pDay;
        private int _pMonth;
        private int _pYear;
        private int _second;

        #endregion

        #region Constructors

        public PersianDateTime()
        {
            _eDateTime = DateTime.Now;

            EnglishYear = _eDateTime.Year;
            EnglishMonth = _eDateTime.Month;
            EnglishDay = _eDateTime.Day;

            _hour = _eDateTime.Hour;
            _minute = _eDateTime.Minute;
            _second = _eDateTime.Second;
            Millisecond = _eDateTime.Millisecond;

            CreatePersianDate();
        }

        public PersianDateTime(DateTime dateTime)
        {
            _eDateTime = dateTime;

            EnglishYear = _eDateTime.Year;
            EnglishMonth = _eDateTime.Month;
            EnglishDay = _eDateTime.Day;

            _hour = _eDateTime.Hour;
            _minute = _eDateTime.Minute;
            _second = _eDateTime.Second;
            Millisecond = _eDateTime.Millisecond;

            CreatePersianDate();
        }

        public PersianDateTime(int persianYear, int persianMonth, int persianDay)
        {
            _pYear = persianYear;
            _pMonth = persianMonth;
            _pDay = persianDay;

            _hour = 0;
            _minute = 0;
            _second = 0;
            Millisecond = 0;

            CreateEnglishDateTime();

            EnglishYear = _eDateTime.Year;
            EnglishMonth = _eDateTime.Month;
            EnglishDay = _eDateTime.Day;
        }

        public PersianDateTime(int persianYear, int persianMonth, int persianDay, int hour, int minute, int second)
        {
            _pYear = persianYear;
            _pMonth = persianMonth;
            _pDay = persianDay;

            _hour = hour;
            _minute = minute;
            _second = second;

            CreateEnglishDateTime();

            EnglishYear = _eDateTime.Year;
            EnglishMonth = _eDateTime.Month;
            EnglishDay = _eDateTime.Day;

            Millisecond = _eDateTime.Millisecond;
        }

        public PersianDateTime(int persianYear, int persianMonth, int persianDay, int hour, int minute, int second,
                               int millisecond)
        {
            _pYear = persianYear;
            _pMonth = persianMonth;
            _pDay = persianDay;

            _hour = hour;
            _minute = minute;
            _second = second;
            Millisecond = millisecond;

            CreateEnglishDateTime();

            EnglishYear = _eDateTime.Year;
            EnglishMonth = _eDateTime.Month;
            EnglishDay = _eDateTime.Day;
        }

        public PersianDateTime(int year, int month, int day, bool isPersianDate)
        {
            _hour = 0;
            _minute = 0;
            _second = 0;
            Millisecond = 0;

            if (isPersianDate)
            {
                _pYear = year;
                _pMonth = month;
                _pDay = day;

                CreateEnglishDateTime();

                EnglishYear = _eDateTime.Year;
                EnglishMonth = _eDateTime.Month;
                EnglishDay = _eDateTime.Day;
            }
            else
            {
                EnglishYear = year;
                EnglishMonth = month;
                EnglishDay = day;

                _eDateTime = new DateTime(year, month, day);

                CreatePersianDate();
            }
        }

        public PersianDateTime(int year, int month, int day, int hour, int minute, int second, bool isPersianDate)
        {
            _hour = hour;
            _minute = minute;
            _second = second;

            if (isPersianDate)
            {
                _pYear = year;
                _pMonth = month;
                _pDay = day;

                CreateEnglishDateTime();

                EnglishYear = _eDateTime.Year;
                EnglishMonth = _eDateTime.Month;
                EnglishDay = _eDateTime.Day;
            }
            else
            {
                EnglishYear = year;
                EnglishMonth = month;
                EnglishDay = day;

                _eDateTime = new DateTime(year, month, day);

                CreatePersianDate();
            }

            Millisecond = _eDateTime.Millisecond;
        }

        public PersianDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond,
                               bool isPersianDate)
        {
            _hour = hour;
            _minute = minute;
            _second = second;

            if (isPersianDate)
            {
                _pYear = year;
                _pMonth = month;
                _pDay = day;

                CreateEnglishDateTime();

                EnglishYear = _eDateTime.Year;
                EnglishMonth = _eDateTime.Month;
                EnglishDay = _eDateTime.Day;
            }
            else
            {
                EnglishYear = year;
                EnglishMonth = month;
                EnglishDay = day;

                _eDateTime = new DateTime(year, month, day);

                CreatePersianDate();
            }

            Millisecond = millisecond;
        }

        public PersianDateTime(string persianDate)
        {
            int dotIndex = persianDate.IndexOf(':');
            if (dotIndex > -1)
            {
                string time = persianDate.Remove(0, dotIndex - 2);
                persianDate = persianDate.Remove(dotIndex - 2);
                string[] t = time.Split(':');
                _hour = Convert.ToInt32(t[0]);
                _minute = Convert.ToInt32(t[1]);
                _second = 0; // Convert.ToInt32(t[2]);
            }
            persianDate = persianDate.Trim();
            string[] d = persianDate.Split('/');
            _pDay = Convert.ToInt32(d[2]); // Convert.ToInt32(d[0]);
            _pMonth = Convert.ToInt32(d[1]);
            _pYear = Convert.ToInt32(d[0]); //Convert.ToInt32(d[2]);

            CreateEnglishDateTime();
        }

        public PersianDateTime(string date, bool isPersianDate)
        {
        }

        #endregion

        #region Properties

        public DateTime EnglishDateTime
        {
            get { return _eDateTime; }
            set { _eDateTime = value; }
        }

        public int PersianYear
        {
            get { return _pYear; }
            set { _pYear = value; }
        }

        public int PersianMonth
        {
            get { return _pMonth; }
            set { _pMonth = value; }
        }

        public int PersianDay
        {
            get { return _pDay; }
            set { _pDay = value; }
        }

        public int EnglishYear { get; set; }

        public int EnglishMonth { get; set; }

        public int EnglishDay { get; set; }

        public int Hour
        {
            get { return _hour; }
            set { _hour = value; }
        }

        public int Minute
        {
            get { return _minute; }
            set { _minute = value; }
        }

        public int Second
        {
            get { return _second; }
            set { _second = value; }
        }

        public int Millisecond { get; set; }

        public string PersianDayOfWeek
        {
            get { return GetPersianDayOfWeek(); }
        }

        public AmPm AmPmState
        {
            get { return GetAmPm(); }
        }

        public string PersianAmPmState
        {
            get { return GetSobhAasr(false); }
        }

        public string FullPersianAmPmState
        {
            get { return GetSobhAasr(true); }
        }

        #endregion

        #region Public Methods

        public string ToFormatedString(PersianDateTimeToStringFormat format)
        {
            switch (format)
            {
                case PersianDateTimeToStringFormat.CammaDate:
                    return _pYear + "," + _pMonth + "," + _pDay;
                case PersianDateTimeToStringFormat.DashDate:
                    return _pYear + "-" + _pMonth + "-" + _pDay;
                case PersianDateTimeToStringFormat.DotDate:
                    return _pYear + "." + _pMonth + "." + _pDay;
                case PersianDateTimeToStringFormat.SlashDate:
                    return _pYear + "/" + _pMonth + "/" + _pDay;
                case PersianDateTimeToStringFormat.UnderlineDate:
                    return _pYear + "_" + _pMonth + "_" + _pDay;
                case PersianDateTimeToStringFormat.WithMonthNameDate:
                    return _pDay + " " + FullPersianMonthsName[_pMonth - 1] + " " + _pYear;
                case PersianDateTimeToStringFormat.TwelveTime:
                    return _minute.ToString().PadLeft(2, '0') + " : " + GetTewlveHour().ToString().PadLeft(2, '0') + " " + GetSobhAasr(true);
                case PersianDateTimeToStringFormat.SimpleDateTimeWithPersianNumbers:
                    return GetSimpleDateTimeWithPersianNumbers();
                case PersianDateTimeToStringFormat.SimpleDateWithPersianNumbers:
                    return GetSimpleDateWithPersianNumbers();
                case PersianDateTimeToStringFormat.MonthNameDateWithPersianNumbers:
                    return GetMonthNameDateWithPersianNumbers();
                case PersianDateTimeToStringFormat.MonthNameDateTimeWithPersianNumbers:
                    return GetMonthNameDateTimeWithPersianNumbers();
                case PersianDateTimeToStringFormat.MonthNameDate:
                    return GetMonthNameDate();
                case PersianDateTimeToStringFormat.MonthNameDateTime:
                    return GetMonthNameDateTime();
            }
            return "";
        }

        #endregion

        #region Static Methods/Properties

        public static PersianDateTime PersianNow
        {
            get { return new PersianDateTime(DateTime.Now); }
        }

        public static int LongMaxYear
        {
            get { return Int16.MaxValue; }
        }

        public static int LongMinYear
        {
            get { return 1; }
        }

        public static int ShortMaxYear
        {
            get { return 1400; }
        }

        public static int ShortMinYear
        {
            get { return 1300; }
        }

        public static string[] ShortPersianDaysOfWeekName
        {
            get { return new[] { "ش", "ی", "د", "س", "چ", "پ", "ج" }; }
        }

        public static string[] MediumPersianDaysOfWeekName
        {
            get { return new[] { "شنب", "یک", "دو", "سه", "چهار", "پنج", "جمع" }; }
        }

        public static string[] FullPersianDaysOfWeekName
        {
            get { return new[] { "شنبه", "یکشنبه", "دوشنبه", "سه شنبه", "چهارشنبه", "پنج شنبه", "جمعه" }; }
        }

        public static string[] ShortPersianMonthsName
        {
            get { return new[] { "فر", "ار", "خر", "تی", "مر", "شه", "مه", "آب", "آذ", "دی", "به", "اس" }; }
        }

        public static string[] MediumPersianMonthsName
        {
            get { return new[] { "فرور", "اردی", "خردا", "تیر", "مردا", "شهری", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفن" }; }
        }

        public static string[] FullPersianMonthsName
        {
            get
            {
                return new[]
                                   {
                               "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی",
                               "بهمن"
                               , "اسفند"
                           };
            }
        }

        public static int GetDaysInPersianMonth(int persianYear, int persianMonth)
        {
            var pc = new PersianCalendar();
            return pc.GetDaysInMonth(persianYear, persianMonth);
        }

        public static string GetTimeWithPersianNumbers(int hour, int minute)
        {
            return NumberUtil.ToPersian(minute.ToString().PadLeft(2, '0')) + " : " + NumberUtil.ToPersian(hour.ToString().PadLeft(2, '0'));
        }

        public static DateTime GetMinEnglishSupportedDateTime()
        {
            return new PersianCalendar().MinSupportedDateTime;
        }

        public static DateTime GetMaxEnglishSupportedDateTime()
        {
            return new PersianCalendar().MaxSupportedDateTime;
        }

        #endregion

        #region Private Methods

        private void CreatePersianDate()
        {
            //DateTime dt = AdjustForTimezone(this._eDateTime, 3.5);
            var dt = _eDateTime;

            var pcal = new PersianCalendar();
            _pYear = pcal.GetYear(dt);
            _pMonth = pcal.GetMonth(dt);
            _pDay = pcal.GetDayOfMonth(dt);
        }

        private void CreateEnglishDateTime()
        {
            _eDateTime = new DateTime(_pYear, _pMonth, _pDay, _hour, _minute, _second, new PersianCalendar());
        }

        private DateTime AdjustForTimezone(DateTime dtAdjust, double DbTimeZoneOffset)
        {
            return dtAdjust.AddHours(3.5 - DbTimeZoneOffset);
        }

        private int GetDaysInMonth(int year, int month)
        {
            var pc = new PersianCalendar();
            return pc.GetDaysInMonth(year, month);
        }

        private string GetPersianDayOfWeek()
        {
            var daysName = GetFullPersianDaysOfWeekName();
            switch (_eDateTime.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                    return daysName[0];
                case DayOfWeek.Sunday:
                    return daysName[1];
                case DayOfWeek.Monday:
                    return daysName[2];
                case DayOfWeek.Tuesday:
                    return daysName[3];
                case DayOfWeek.Wednesday:
                    return daysName[4];
                case DayOfWeek.Thursday:
                    return daysName[5];
                case DayOfWeek.Friday:
                    return daysName[6];
            }
            return "";
        }

        private AmPm GetAmPm()
        {
            if (_eDateTime.TimeOfDay.TotalDays < 0.5)
                return AmPm.Am;
            else
                return AmPm.Pm;
        }

        private string GetSobhAasr(bool isFullName)
        {
            if (GetAmPm() == AmPm.Am)
                return (isFullName) ? "صبح" : "ص";
            return (isFullName) ? "عصر" : "ع";
        }

        private int GetTewlveHour()
        {
            int hour = _hour;
            if (hour == 0)
                hour = 12;
            else if (hour > 12)
                hour -= 12;
            return hour;
        }

        private string GetSimpleDateTimeWithPersianNumbers()
        {
            return NumberUtil.ToPersian(_minute.ToString().PadLeft(2, '0')) + " : " +
                   NumberUtil.ToPersian(_hour.ToString().PadLeft(2, '0')) + "&nbsp;&nbsp;,&nbsp;&nbsp;" +
                   NumberUtil.ToPersian(_pDay + "/" + _pMonth + "/" + _pYear);
        }

        private string GetSimpleDateWithPersianNumbers()
        {
            return NumberUtil.ToPersian(_pDay + "/" + _pMonth + "/" + _pYear);
        }

        private string GetMonthNameDateWithPersianNumbers()
        {
            return NumberUtil.ToPersian(_pDay + " " + FullPersianMonthsName[_pMonth - 1] + " " + _pYear);
        }

        private string GetMonthNameDateTimeWithPersianNumbers()
        {
            return GetMonthNameDateWithPersianNumbers() + "<br />" +
              NumberUtil.ToPersian(_minute.ToString().PadLeft(2, '0')) + " : " + NumberUtil.ToPersian(_hour.ToString().PadLeft(2, '0'));
        }

        private string GetMonthNameDate()
        {
            return _pDay + " " + FullPersianMonthsName[_pMonth - 1] + " " + _pYear;
        }

        private string GetMonthNameDateTime()
        {
            return GetMonthNameDate() + "  " + _minute + " : " + _hour;
        }


        private string[] GetShortPersianDaysOfWeekName()
        {
            return new[] { "ش", "ی", "د", "س", "چ", "پ", "ج" };
        }

        private string[] GetMediumPersianDaysOfWeekName()
        {
            return new[] { "شنب", "یک", "دو", "سه", "چهار", "پنج", "جمع" };
        }

        private string[] GetFullPersianDaysOfWeekName()
        {
            return new[] { "شنبه", "یکشنبه", "دوشنبه", "سه شنبه", "چهارشنبه", "پنج شنبه", "جمعه" };
        }

        private string[] GetShortPersianMonthsName()
        {
            return new[] { "فر", "ار", "خر", "تی", "مر", "شه", "مه", "آب", "آذ", "دی", "به", "اس" };
        }

        private string[] GetMediumPersianMonthsName()
        {
            return new[] { "فرور", "اردی", "خردا", "تیر", "مردا", "شهری", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفن" };
        }

        private string[] GetFullPersianMonthsName()
        {
            return new[] { "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند" };
        }

        #endregion
    }

    public enum PersianDateTimeToStringFormat
    {
        DashDate,
        SlashDate,
        DotDate,
        CammaDate,
        UnderlineDate,
        WithMonthNameDate,
        TwelveTime,
        MonthNameDate,
        MonthNameDateTime,
        SimpleDateTimeWithPersianNumbers,
        SimpleDateWithPersianNumbers,
        MonthNameDateWithPersianNumbers,
        MonthNameDateTimeWithPersianNumbers,
    }

    public enum AmPm
    {
        Am = 0,
        Pm = 1,
    }
}
