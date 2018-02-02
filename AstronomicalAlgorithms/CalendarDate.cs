
using System;

namespace AstronomicalAlgorithms
{
    public class CalendarDate
    {
        public int Year { get; }
        public Month Month { get; }
        public double Day { get; }

        public CalendarDate(int year, Month month, double day)
        {
            Year = year;
            Month = month;
            Day = day;
        }

        public static CalendarDate FromDayOfYear(int year, int dayOfYear)
        {
            var k = DayOfYearConstant(year);
            var month = dayOfYear < 32
                ? 1
                : (int) ((9D * (k + dayOfYear)) / 275 + 0.98);

            var day = dayOfYear - (275 * month / 9) + k * ((month + 9) / 12) + 30;
            
            return new CalendarDate(year, (Month)month, day);
        }

        public static CalendarDate FromDateTime(DateTime dateTime)
        {
            var year = dateTime.Year;
            var month = dateTime.Month;
            var day = GetDecimalDay(dateTime);

            return new CalendarDate(year, (Month)month, day);
        }

        public static bool IsLeapYear(int year)
        {
            var centurialYear = year % 100 == 0;

            if (IsGregorian(year, 0, 0) && centurialYear) return year % 400 == 0;

            return year % 4 == 0;
        }

        public static bool IsValidGregorianDate(int year, Month month, double day)
        {
            return !(AfterLastJulianDay(year, (int)month, day) && BeforeFirstGregorianDay(year, (int)month, day));
        }

        public static bool IsGregorian(int year, Month month, double day)
        {
            return
                IsValidGregorianDate(year, month, day)
                && AfterLastJulianDay(year, (int)month, day);
        }

        public static bool IsJulian(int year, Month month, double day)
        {
            return
                IsValidGregorianDate(year, month, day)
                && BeforeFirstGregorianDay(year, (int)month, day);
        }

        private static double GetDecimalDay(DateTime dateTime)
        {
            return dateTime.Day
                   + dateTime.Hour / 24
                   + dateTime.Minute / (24 * 60)
                   + dateTime.Second / (24 * 60 * 60);
        }

        private static bool BeforeFirstGregorianDay(int year, int month, double day)
        {
            const int startYear = 1582;
            const int startMonth = 10;
            const int startDay = 15;

            var earlierYear = year < startYear;
            var earlierMonth = year == startYear && month < startMonth;
            var earlierDay =
                year == startYear
                && month == startMonth
                && day < startDay;

            return earlierYear || earlierMonth || earlierDay;
        }

        private static bool AfterLastJulianDay(int year, int month, double day)
        {
            const int endYear = 1582;
            const int endMonth = 10;
            const int endDay = 5;

            var laterYear = year > endYear;
            var laterMonth = year == endYear && month > endMonth;
            var laterDay =
                year == endYear
                && month == endMonth
                && day >= endDay;

            return laterYear || laterMonth || laterDay;
        }

        public Day WeekDay()
        {
            return ToJulianDay().WeekDay();
        }

        public JulianDay ToJulianDay()
        {
            return new JulianDay(Year, Month, Day);
        }

        public int DayOfYear()
        {
            var k = DayOfYearConstant(Year);

            var m = (int) Month;

            return 275 * m / 9 - k * ((m + 9) / 12) + (int) Day - 30;
        }

        private static int DayOfYearConstant(int year)
        {
            var k = IsLeapYear(year)
                ? 1
                : 2;
            return k;
        }
    }
}