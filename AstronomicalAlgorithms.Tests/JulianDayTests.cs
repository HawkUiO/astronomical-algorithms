using System;
using Xunit;

namespace AstronomicalAlgorithms.Tests
{
    public class JulianDayTests
    {
        [Theory]
        [InlineData(1977, Month.April, 26.4, 2_443_259.9)]
        [InlineData(1957, Month.October, 4.81, 2436_116.31)]
        [InlineData(333, Month.January, 27.5, 1_842_713.0)]
        [InlineData(2000, Month.January, 1.5, 2_451_545.0)]
        [InlineData(1999, Month.January, 1.0, 2_451_179.5)]
        [InlineData(1987, Month.January, 27, 2_446_822.5)]
        [InlineData(1987, Month.June, 19.5, 2_446_966.0)]
        [InlineData(1988, Month.January, 27, 2_447_187.5)]
        [InlineData(1988, Month.June, 19.5, 2_447_332.0)]
        [InlineData(1900, Month.January, 1.0, 2_415_020.5)]
        [InlineData(1600, Month.January, 1.0, 2_305_447.5)]
        [InlineData(1600, Month.December, 31.0, 2_305_812.5)]
        [InlineData(837, Month.April, 10.3, 2_026_871.8)]
        [InlineData(-123, Month.December, 31, 1_676_496.5)]
        [InlineData(-122, Month.January, 1.0, 1_676_497.5)]
        [InlineData(-1000, Month.July, 12.5, 1_356_001.0)]
        [InlineData(-1000, Month.February, 29.0, 1_355_866.5)]
        [InlineData(-1001, Month.August, 17.9, 1_355_671.4)]
        [InlineData(-4712, Month.January, 1.5, 0.0)]
        public void Examples(int year, Month month, double day, double expected)
        {
            var julianDay = new JulianDay(year, month, day);
            Assert.Equal(expected, julianDay.Value, 1);
        }

        [Theory]
        [InlineData(1582, Month.October, 6)]
        [InlineData(1582, Month.October, 7)]
        [InlineData(1582, Month.October, 8)]
        [InlineData(1582, Month.October, 9)]
        [InlineData(1582, Month.October, 10)]
        [InlineData(1582, Month.October, 11)]
        [InlineData(1582, Month.October, 12)]
        [InlineData(1582, Month.October, 13)]
        [InlineData(1582, Month.October, 14)]
        public void NonExisitingDates(int year, Month month, double day)
        {
            Assert.Throws<ArgumentException>(() => new JulianDay(year, month, day));
        }

        [Theory]
        [InlineData(1581, Month.October, 10)]
        [InlineData(1581, Month.October, 12)]
        [InlineData(1581, Month.October, 30)]
        [InlineData(1582, Month.September, 10)]
        [InlineData(1582, Month.September, 30)]
        [InlineData(1582, Month.October, 3)]
        [InlineData(1582, Month.October, 4)]
        [InlineData(1582, Month.October, 4.99)]
        [InlineData(1582, Month.October, 15)]
        [InlineData(1582, Month.October, 16)]
        [InlineData(1582, Month.November, 1)]
        [InlineData(1583, Month.October, 1)]
        [InlineData(1583, Month.October, 10)]
        [InlineData(1583, Month.October, 12)]
        public void DatesAroundChangeOfcalendar(int year, Month month, double day)
        {
            new JulianDay(year, month, day);
        }

        [Fact]
        public void ModifiedJulianDate()
        {
            var julianDay = new JulianDay(1988, Month.June, 19.5);
            Assert.Equal(47331.5, julianDay.Modified, 1);
        }

        [Theory]
        [InlineData(2_436_116.31, 1957, Month.October, 4.81)]
        [InlineData(1_842_713.0, 333, Month.January, 27.5)]
        [InlineData(1_507_900.13, -584, Month.May, 28.63)]
        public void ToCalendardate(double julianDay, int year, Month month, double day)
        {
            var calendardate = new JulianDay(julianDay).ToCalendarDate();
            var expected = new CalendarDate(year, month, day);

            Assert.Equal(expected.Year, calendardate.Year);
            Assert.Equal(expected.Month, calendardate.Month);
            Assert.Equal(expected.Day, calendardate.Day, 8);
        }
    }
}
