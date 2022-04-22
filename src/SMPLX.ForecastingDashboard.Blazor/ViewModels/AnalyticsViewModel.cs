using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise.Extensions;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.Extensions.Options;
using SMPLX.ForecastingDashboard.Cases;
using SMPLX.ForecastingDashboard.ForecastData;
using SMPLX.ForecastingDashboard.Helpers;
using SMPLX.ForecastingDashboard.Settings;

namespace SMPLX.ForecastingDashboard.Blazor.ViewModels;

public class AnalyticsViewModel : ForecastingDashboardComponentBase
{
    private IEnumerable<CaseDto> Cases { get; set; }

    protected IEnumerable<MonthlyCaseDto> MonthlyCases { get; set; }
    protected IEnumerable<MonthlyCaseDto> MonthlyCasesNoForecast
    {
        get
        {
            if (MonthlyCases == null) return new List<MonthlyCaseDto>();
            return MonthlyCases.Where(c => !c.IsForecast && c.Count>0);
        }
    }

    protected double MAPE
    {
        get
        {
            if (MonthlyCases == null || !MonthlyCases.Any()) return 0;
            return Math.Round(MonthlyCasesNoForecast.Select(c => c.ErrorPercent).Average(),2);
        }
    }

    private IOptions<GeologicalOptions> _geoOptions;
    private IOptions<GeologicalOptions> GeoOptions=> LazyGetRequiredService(ref _geoOptions);
    protected DateTime StartDate
    {
        get
        {
            if(MonthlyCases.IsNullOrEmpty())return DateTime.Now.AddYears(-1);
            return MonthlyCases?.LastOrDefault().GetDate().AddYears(-2) ?? DateTime.Now.AddYears(-1);
        }
    }

    protected DateTime EndDate
    {
        get
        {
            if(MonthlyCases.IsNullOrEmpty())return DateTime.Now;
            return MonthlyCases?.LastOrDefault().GetDate() ?? DateTime.Now;
        }
    }

    private ICaseAppService CaseAppService => LazyGetRequiredService(ref _caseAppService);
    
    private ICaseAppService _caseAppService;

    protected IReadOnlyList<String> Baranggays = new List<string>(){"All"};

    public AnalyticsViewModel()
    {
    }
    protected override async Task OnInitializedAsync()
    {
        await InitData();
    }

    public async Task InitData()
    {
        var res = await CaseAppService.GetListAsync(new CaseGetListDto());
        Cases = res.Items;
        MonthlyCases = CaseAnalyticsHelper.GetMonthlyAccumulatedCases(Cases);
        var b = Cases.Select(c=>c.Barangay).Distinct().OrderBy(_=>_).ToList();
        b.AddFirst("All");
        Baranggays = b;
    }

    protected void OnBaranggaySelect(object args, string dropdown)
    {
        var baranggay = (string) args;
        if (baranggay.ToLower() != "all")
        { 
            MonthlyCases = CaseAnalyticsHelper.GetMonthlyAccumulatedCases(Cases.Where(c => c.Barangay.ToLower() == baranggay.ToLower()));
            // if (baranggay.ToLower() == "poblacion")
            // {
            //     MonthlyCases = CaseAnalyticsHelper.GetMonthlyAccumulatedCases(Cases.Where(c => c.Barangay.ToLower().StartsWith(baranggay.ToLower())));
            // }
            // else
            // {
            //     MonthlyCases = CaseAnalyticsHelper.GetMonthlyAccumulatedCases(Cases.Where(c => c.Barangay.ToLower() == baranggay.ToLower()));
            // }
        }
        else
        {
            MonthlyCases = CaseAnalyticsHelper.GetMonthlyAccumulatedCases(Cases);
        }

    }
}

public class CaseAnalyticsHelper
{
    public static List<MonthlyCaseDto> GetMonthlyAccumulatedCases(IEnumerable<CaseDto> cases)
    {
        if (cases.IsNullOrEmpty()) return new List<MonthlyCaseDto>();
        var monthlyCases = new List<MonthlyCaseDto>();
        var minDate = cases.Min(_ => _.DateRegistered);
        var maxDate = cases.Max(_ => _.DateRegistered);
        int mc = 1;

        //Set per month accumulation
        for (DateTime i = minDate; i <= maxDate; i = i.AddMonths(1))
        {
            var a = cases.Count(_ => _.DateRegistered.Month == i.Month && _.DateRegistered.Year == i.Year);
            monthlyCases.Add(new MonthlyCaseDto(mc++, i.Year, i.Month, Convert.ToDouble(a)));
        }
        
        //Get the intercept
        var monthlyCasesCounts = monthlyCases.Select(_ => _.Count).ToArray();
        var monthlyPeriods = monthlyCases.Select(_ => _.Period).ToArray();
        var intercept = MathUtil.Intercept(monthlyCasesCounts, monthlyPeriods);
        var slope = MathUtil.Slope(monthlyCasesCounts, monthlyPeriods);
        var sindex = Enumerable.Range(1, 12).ToDictionary(x => x,
            x => monthlyCases.Where(m => m.Month == x).Select(_ => _.Count).Average() /
                 monthlyCasesCounts.Average());

        foreach (var item in monthlyCases)
        {
            item.LinearTrend = (intercept + slope * item.Period).RoundOff(2);
            item.SeasonalityTrend = sindex[item.Month] * item.LinearTrend;
        }

        //Add the 1 year forecast
        var lastCase  = monthlyCases.LastOrDefault();
        if (lastCase != null)
        {        
            var forecastDate = lastCase.GetDate();
            
            for (int m = 1; m <= 12; m++)
            {
                var fd = forecastDate.AddMonths(m);
                var mcf = new MonthlyCaseDto(mc++, fd.Year, fd.Month, 0, true);
                mcf.LinearTrend = (intercept + slope * mcf.Period).RoundOff(2);
                mcf.SeasonalityTrend =  sindex[mcf.Month] * mcf.LinearTrend;
                monthlyCases.Add(mcf);
                
            }
        }

        return monthlyCases;
    }
}
