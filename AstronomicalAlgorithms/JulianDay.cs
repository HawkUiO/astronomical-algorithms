using System;
using static System.Math;

namespace AstronomicalAlgorithms
{
    /// <summary>
    /// </summary>
    /// <remarks>
    ///     I the methods described below, the Gregorian calendar
    ///     reform is taken into account. Thus, the day following
    ///     1582 October 4 (Julian calendar) is 1582 October 15
    ///     (Gregorian calendar).
    /// </remarks>
    public class JulianDay
    {
        private const double GreenwichMeanMidnightCorrecton = -2_400_000.5;
        private readonly double _day;
        private readonly bool _isJulian;
        private readonly int _month;
        private readonly int _year;

        public JulianDay(double value)
        {
            Value = value;
        }

        public JulianDay(int year, Month month, double day)
        {
            var monthNumber = (int) month;

            if (!CalendarDate.IsValidGregorianDate(year, month, day))
            {
                throw new ArgumentException("CalendarDate is between Julian and Gregorian calendar.");
            }

            _isJulian = CalendarDate.IsJulian(year, month, day);
            _year = GetUpdatedYear(year, monthNumber);
            _month = GetUpdatedMonth(monthNumber);
            _day = day;
            Value = GetJulianDay();
        }

        public static JulianDay FromCalendarDate(CalendarDate calendarDate)
        {
            return new JulianDay(calendarDate.Year, calendarDate.Month, calendarDate.Day);
        }

        public static JulianDay FromYear(int year)
        {
            return new JulianDay(year, Month.January, 0);
        }

        public static JulianDay FromDatetime(DateTime dateTime)
        {
            var calendarDate = CalendarDate.FromDateTime(dateTime);
            return FromCalendarDate(calendarDate);
        }

        public CalendarDate ToCalendarDate()
        {
            if (Value < 0)
            {
                throw new Exception("Not valid on negative Julian days.");
            }

            var increasedJd = Value + 0.5;
            var z = Floor(increasedJd);
            var a = z;
            if (z >= 2299161)
            {
                var alpha = Floor((z - 1867216.25) / 36524.25);
                a = z + 1 + alpha - Floor(alpha / 4);
            }

            var b = a + 1524;
            var c = (int)Floor((b - 122.1) / 365.25);
            var d = Floor(365.25 * c);
            var e = Floor((b - d) / 30.6001);
            var f = increasedJd - z;

            var day = b - d - Floor(30.6001 * e) + f;
            var month = e < 14
                ? e - 1
                : e - 13;
            var year = month > 2
                ? c - 4716
                : c - 4715;

            return new CalendarDate(year, (Month)month, day);
        }

        public Day WeekDay()
        {
            return (Day)((Value + 1.5) % 7);
        }

        public double Value { get; }

        public double Modified => Value + GreenwichMeanMidnightCorrecton;

        private double GetJulianDay()
        {
            var gregorianCorrection = GetGregorianCorrection();
            return Floor(365.25 * (_year + 4716))
                   + Floor(30.6001 * (_month + 1))
                   + _day
                   + gregorianCorrection
                   - 1524.5;
        }

        private double GetGregorianCorrection()
        {
            if (_isJulian) return 0;
            var a = _year / 100;
            return 2 - a + a / 4;
        }

        private static int GetUpdatedYear(int year, int month)
        {
            return KeepUnchanged(month)
                ? year
                : year - 1;
        }

        private static int GetUpdatedMonth(int month)
        {
            return KeepUnchanged(month)
                ? month
                : month + 12;
        }

        private static bool KeepUnchanged(int month)
        {
            return month > 2;
        }
    }
}