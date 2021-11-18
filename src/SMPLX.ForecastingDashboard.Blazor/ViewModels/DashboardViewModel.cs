using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorDateRangePicker;
using Blazorise.Charts;
using NUglify.Helpers;
using SMPLX.ForecastingDashboard.Cases;
using SMPLX.ForecastingDashboard.ForecastData;
using SMPLX.ForecastingDashboard.Helpers;
using Volo.Abp.DependencyInjection;

namespace SMPLX.ForecastingDashboard.Blazor.ViewModels
{
    public class DashboardViewModel : ForecastingDashboardComponentBase
    {
        protected IReadOnlyList<CaseDto> Cases { get; set; }
        protected ICaseAppService CaseAppService => LazyGetRequiredService(ref _caseAppService);
        protected DateTime OldestRecordDate = DateTime.Now.AddYears(-5);

        private ICaseAppService _caseAppService;


        public DashboardViewModel()
        {
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadCaseData();
            OldestRecordDate = Cases.Min(_ => _.DateRegistered);
        }

        List<MonthlyCaseDto> monthlyForecast = new List<MonthlyCaseDto>();

        protected async Task LoadCaseData()
        {
            var res = await CaseAppService.GetListAsync(new CaseGetListDto());

            Cases = res.Items;
            var minDate = Cases.Min(_ => _.DateRegistered);
            var maxDate = Cases.Max(_ => _.DateRegistered);
            monthlyForecast = new List<MonthlyCaseDto>();
            int mc = 1;

            //Set per month accumulation
            for (DateTime i = minDate; i <= maxDate; i = i.AddMonths(1))
            {
                var a = Cases.Count(_ => _.DateRegistered.Month == i.Month && _.DateRegistered.Year == i.Year);
                monthlyForecast.Add(new MonthlyCaseDto(mc++, i.Year, i.Month, Convert.ToDouble(a)));
            }

            //Get the intercept
            var monthlyCasesCounts = monthlyForecast.Select(_ => _.Count).ToArray();
            var monthlyPeriods = monthlyForecast.Select(_ => _.Period).ToArray();
            var intercept = MathUtil.Intercept(monthlyCasesCounts, monthlyPeriods);
            var slope = MathUtil.Slope(monthlyCasesCounts, monthlyPeriods);
            var sindex = Enumerable.Range(1, 12).ToDictionary(x => x,
                x => monthlyForecast.Where(m => m.Month == x).Select(_ => _.Count).Average() /
                     monthlyCasesCounts.Average());

            foreach (var item in monthlyForecast)
            {
                item.LinearTrend = (intercept + slope * item.Period).RoundOff(2);
                item.SeasonalityTrend = sindex[item.Month] * item.LinearTrend;
            }
            //Set per year accumulation
            //Set periods, periods will base on monthly or yearly sequence not on every record
            //Set seasonality index
            //Set linear and seasonality trend
        }

        protected async Task<ChartData<LineChartDataset<double>>> GetAccumulatedCasesAsync(DateRange dr, RangeStep rs)
        {
            var dateFilteredSets = monthlyForecast.Where(_ =>
            {
                var d = new DateTime(_.Year, _.Month, 1);
                return d >= dr.Start && d <= dr.End;
            });
            var lcDataset = new LineChartDataset<double>
            {
                Label = L["AccumulatedCases"],
                Data = dateFilteredSets.Select(_ => _.Count).ToList(),
                BackgroundColor = ChartColors.Accumulated,
                BorderColor = ChartColors.AccumulatedBorder,
                Fill = true,
                PointRadius = 3,
                PointBackgroundColor = ChartColors.AccumulatedBorder,
            };
            return new ChartData<LineChartDataset<double>>
                { Labels = dateFilteredSets.Select(_ => _.Label).ToArray(), Datasets = lcDataset };
        }

        protected async Task<ChartData<LineChartDataset<double>>> GetLinearTrendAsync(DateRange dr, RangeStep rs)
        {
            //var dateFilteredSets = monthlyForecast.Where(_ => _.DateRegistered >= dr.Start && _.DateRegistered <= dr.End);
            var dateFilteredSets = monthlyForecast.Where(_ =>
            {
                var d = new DateTime(_.Year, _.Month, 1);
                return d >= dr.Start && d <= dr.End;
            });
            if (rs == RangeStep.Monthly)
            {
                var lcDataset = new LineChartDataset<double>
                {
                    Label = L["LinearTrend"],
                    Data = dateFilteredSets.Select(_ => _.LinearTrend).ToList(),
                    BackgroundColor = ChartColors.Linear,
                    BorderColor = ChartColors.LinearBorder,
                    Fill = true,
                    PointRadius = 3,
                    PointBackgroundColor = ChartColors.LinearBorder,
                };
                return new ChartData<LineChartDataset<double>>
                {
                    Labels = dateFilteredSets.Select(_ => _.Label).ToArray(),
                    Datasets = lcDataset
                };
            }
            else
            {
            }

            return new ChartData<LineChartDataset<double>>();
        }

        protected async Task<ChartData<LineChartDataset<double>>> GetSeasonalityTrendAsync(DateRange dr, RangeStep rs)
        {          
            var dateFilteredSets = monthlyForecast.Where(_ =>
            {
                //TODO: set datetime to MonthlyCaseDto later
                var d = new DateTime(_.Year, _.Month, 1);
                return d >= dr.Start && d <= dr.End;
            });
            if (rs == RangeStep.Monthly)
            {
                var lcDataset = new LineChartDataset<double>
                {
                    Label = L["SeasonalTrend"],
                    Data = dateFilteredSets.Select(_ => _.SeasonalityTrend).ToList(),
                    BackgroundColor = ChartColors.Seasonal,
                    BorderColor = ChartColors.SeasonalBorder,
                    Fill = true,
                    PointRadius = 3,
                    PointBackgroundColor = ChartColors.SeasonalBorder,
                };
                return new ChartData<LineChartDataset<double>>
                {
                    Labels = dateFilteredSets.Select(_ => _.Label).ToArray(),
                    Datasets = lcDataset
                };
            }
            else
            {
            }

            return new ChartData<LineChartDataset<double>>();
        }

        // protected async Task<ChartData> GetPerBarangayCases(DateRange dr, RangeStep rs)
        // {
        //     return new ChartData();
        // }
        //
        // protected async Task<ChartData> GetLifeStatusChartData(DateRange dr, RangeStep rs)
        // {
        //     return new ChartData();
        // }
        //
        // protected async Task<ChartData> GetGenderChartData(DateRange dr, RangeStep rs)
        // {
        //     return new ChartData();
        // }
        //
        // protected async Task<ChartData> GetAgeChartData(DateRange dr, RangeStep rs)
        // {
        //     return new ChartData();
        // }

        protected string FlattenString(string s) => s.ToLower().Replace(" ", "");

        protected bool FlatMatch(string baseString, string searchString) =>
            FlattenString(baseString).StartsWith(FlattenString(searchString));
    }

    public static class ChartColors
    {
        public static readonly string Accumulated = ChartColor.FromRgba(40, 142, 202, 0.2f);
        public static readonly string Linear = ChartColor.FromRgba(140, 94, 255, 0.0f);
        public static readonly string Seasonal = ChartColor.FromRgba(75, 192, 192, 0.0f);

        public static readonly string AccumulatedBorder = ChartColor.FromRgba(40, 142, 202, 1f);
        public static readonly string LinearBorder = ChartColor.FromRgba(140, 94, 255, 1f);
        public static readonly string SeasonalBorder = ChartColor.FromRgba(75, 192, 192, 1f);
    }


    public enum RangeStep
    {
        Monthly,
        Yearly
    }

    public class ChartData<TDatasetType>
    {
        public string[] Labels { get; set; }
        public TDatasetType Datasets { get; set; }
    }
}