using System;
using System.Globalization;
using System.Linq;

namespace SMPLX.ForecastingDashboard.Common
{
    public class DataConsts
    {
        public static readonly string[] Months = Enumerable.Range(1, 12)
            .Select(i => DateTimeFormatInfo.CurrentInfo.GetMonthName(i)).ToArray();
        
        public static readonly string[] Days = new string[]
            {
                DateTimeFormatInfo.CurrentInfo.GetDayName(DayOfWeek.Monday),
                DateTimeFormatInfo.CurrentInfo.GetDayName(DayOfWeek.Tuesday)  ,
                DateTimeFormatInfo.CurrentInfo.GetDayName(DayOfWeek.Wednesday)  ,
                DateTimeFormatInfo.CurrentInfo.GetDayName(DayOfWeek.Thursday)  ,
                DateTimeFormatInfo.CurrentInfo.GetDayName(DayOfWeek.Friday)  ,
                DateTimeFormatInfo.CurrentInfo.GetDayName(DayOfWeek.Saturday)  ,
                DateTimeFormatInfo.CurrentInfo.GetDayName(DayOfWeek.Sunday)  
            };
    }
}