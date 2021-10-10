namespace SMPLX.ForecastingDashboard.ForecastData
{
    public interface IMonthlyCasesData
    {
        public int Week { get; }
        public string Month { get; }
        public double TotalCases { get; }
    }
}