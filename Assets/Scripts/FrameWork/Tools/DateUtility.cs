using System;

namespace Framework.Tools
{
    public static class DateUtility
    {
        public static string DateFormat(DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }

        public static int DaysBetween(DateTime start, DateTime end)
        {
            return (end - start).Days;
        }

        public static int DaysInMonth(int year, int month)
        {
            return DateTime.DaysInMonth(year, month);
        }

        // ... 其他日期处理方法
    }
}