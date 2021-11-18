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

        private string label;
        public double Count { get; }

        public double LinearTrend { get; set; }
        public double SeasonalityTrend { get; set; }


        public MonthlyCaseDto(double period, int year, int month, double count)
        {
            Period = period;
            Year = year;
            Month = month;
            Count = count;
        }
    }
}