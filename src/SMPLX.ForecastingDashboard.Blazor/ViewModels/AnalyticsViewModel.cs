using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using SMPLX.ForecastingDashboard.Cases;
using SMPLX.ForecastingDashboard.ForecastData;
using SMPLX.ForecastingDashboard.Helpers;

namespace SMPLX.ForecastingDashboard.Blazor.ViewModels;

public class AnalyticsViewModel : ForecastingDashboardComponentBase
{
    private IEnumerable<CaseDto> Cases { get; set; }

    protected IEnumerable<MonthlyCaseDto> MonthlyCases { get; private set; }

    protected DateTime StartDate => MonthlyCases?.LastOrDefault().GetDate().AddYears(-2) ?? DateTime.Now.AddYears(-1);

    protected DateTime EndDate => MonthlyCases?.LastOrDefault().GetDate() ?? DateTime.Now;
    private ICaseAppService CaseAppService => LazyGetRequiredService(ref _caseAppService);
    
    private ICaseAppService _caseAppService;
    
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
    }
}

public class CaseAnalyticsHelper
{
    public static List<MonthlyCaseDto> GetMonthlyAccumulatedCases(IEnumerable<CaseDto> cases)
    {
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

public class CaseDataPoint : CaseDto
{
    public string Name { get; set; }
}