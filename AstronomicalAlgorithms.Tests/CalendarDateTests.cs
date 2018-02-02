using System;
using Xunit;

namespace AstronomicalAlgorithms.Tests
{
    public class CalendarDateTests
    {

        [Theory]
        [InlineData(900, true)]
        [InlineData(1236, true)]
        [InlineData(750, false)]
        [InlineData(1429, false)]
        [InlineData(1700, false)]
        [InlineData(1800, false)]
        [InlineData(1900, false)]
        [InlineData(2100, false)]
        [InlineData(1600, true)]
        [InlineData(2000, true)]
        [InlineData(2400, true)]
        public void LeapYears(int year, bool expected)
        {
            Assert.Equal(expected, CalendarDate.IsLeapYear(year));
        }

        [Theory]
        [InlineData(1582, Month.April, 4, true)]
        [InlineData(1582, Month.October, 4, true)]
        [InlineData(1582, Month.October, 5, false)]
        [InlineData(1582, Month.October, 14, false)]
        [InlineData(1582, Month.October, 15, true)]
        public void ValidGregorianDates(int year, Month month, double day, bool expected)
        {
            Assert.Equal(expected, CalendarDate.IsValidGregorianDate(year, month, day));
        }

        [Theory]
        [InlineData(1954, Month.June, 30, Day.Wednesday)]
        [InlineData(1582, Month.October, 4, Day.Thursday)]
        [InlineData(1582, Month.October, 15, Day.Friday)]
        public void WeekDay(int year, Month month, double day, Day expected)
        {
            var calendardate = new CalendarDate(year, month, day);
            Assert.Equal(expected, calendardate.WeekDay());
        }

        [Theory]
        [InlineData(1978, Month.November, 14, 318)]
        [InlineData(1988, Month.April, 22, 113)]
        public void DayOfYear(int year, Month month, double day, int expected)
        {
            var calendardate = new CalendarDate(year, month, day);
            Assert.Equal(expected, calendardate.DayOfYear());
        }

        [Theory]
        [InlineData(1978, Month.November, 14, 318)]
        [InlineData(1988, Month.April, 22, 113)]
        public void DateFromdayOfYear(int year, Month month, double day, int dayOfYear)
        {
            var date = CalendarDate.FromDayOfYear(year, dayOfYear);
            Assert.Equal(month, date.Month);
            Assert.Equal(day, date.Day);
        }

    }
}
