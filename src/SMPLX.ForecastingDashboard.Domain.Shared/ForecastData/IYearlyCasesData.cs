namespace SMPLX.ForecastingDashboard.ForecastData
{
    public interface IYearlyCasesData
    {
        public string Month { get; }
        public int Year { get; }
        public double TotalCases { get; }
    }
}