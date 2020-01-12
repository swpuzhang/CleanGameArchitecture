using System;
using System.Collections.Generic;
using System.Text;

namespace Commons.Tools.Time
{
    public static class TimeTool
    {
        public static string ToNormal(this DateTime date)
        {

            return $"{date.Year:0000}{date.Month:00}{date.Day:00}{date.Hour:00}{date.Minute:00}{date.Second:00}";

        }

        public static string ToNormalDay(this DateTime date)
        {
            return $"{date.Year:0000}{date.Month:00}{date.Day:00}";
        }

        public static string ToNormalWeek(this in DateTime date)
        {
            var weekBegin = date;
            weekBegin.AddDays(-(int)date.DayOfWeek);
            return $"{weekBegin.Year:0000}{weekBegin.Month:00}{weekBegin.Day}";
        }


        public static Int64 ToTimeStamp(this DateTime date)
        {
            var seconds = date.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            return Convert.ToInt64(seconds);
        }

        public static DateTime DateOfDayBegin(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }

        public static DateTime DateOfWeekBegin(this DateTime date)
        {
            var weekBegin = date;
            weekBegin.AddDays(-(int)date.DayOfWeek);
            return new DateTime(weekBegin.Year, weekBegin.Month, weekBegin.Day);
        }
    }
}
