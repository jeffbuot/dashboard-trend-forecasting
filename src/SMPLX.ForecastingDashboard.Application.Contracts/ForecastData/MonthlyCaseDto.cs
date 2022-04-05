using System;
using System.Globalization;

namespace SMPLX.ForecastingDashboard.ForecastData
{
    public class MonthlyCaseDto
    {
        public double Period { get; }
        public int Year { get; }
        public int Month { get; }
        public string Label
        {
            get
            {
                if (string.IsNullOrEmpty(label))
                {
                    label = $"{CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(Month)} {Year}";
                }

                return label;
            }
        }

        public bool IsForecast { get; set; }

        private string label;
        public double Count { get; }

        public double LinearTrend { get; set; }
        public double SeasonalityTrend { get; set; }
        
        public double Error => Math.Round(SeasonalityTrend - Count,2);
        public double ErrorPercent => Math.Round(Math.Abs(Error)/Count * 100,2);

        public DateTime GetDate() => new DateTime(Year, Month, 1);

        public MonthlyCaseDto(double period, int year, int month, double count, bool isForecast = false)
        {
            Period = period;
            Year = year;
            Month = month;
            Count = count;
            IsForecast = isForecast;
        }
    }
}