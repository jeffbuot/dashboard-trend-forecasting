using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorDateRangePicker;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using SMPLX.ForecastingDashboard.Cases;
using SMPLX.ForecastingDashboard.ForecastData;
using SMPLX.ForecastingDashboard.Settings;

namespace SMPLX.ForecastingDashboard.Blazor.ViewModels;

public class HeatmapViewModel : ForecastingDashboardComponentBase
{
    protected IEnumerable<CaseDto> Cases { get; set; }
    protected bool IsMapLoading { get; set; }
    protected DateRangePicker HeatmapDateRangePicker;
    protected Dictionary<string, DateRange> DateRanges => new()
    {
        {
            "Overall History", new DateRange
            {
                Start = OldestRecordDate,
                End = DateTime.Now
            }
        },
    };

    private IOptions<GeologicalOptions> _geoOptions;
    private ICaseAppService _caseAppService;
    private IOptions<GeologicalOptions> GeoOptions=> LazyGetRequiredService(ref _geoOptions);
    private ICaseAppService CaseAppService => LazyGetRequiredService(ref _caseAppService);
    
    protected DateTime OldestRecordDate = DateTime.Now.AddYears(-5);
    
    protected override async Task OnInitializedAsync()
    {
        await InitData();
    }

    private async Task InitData()
    {
        var res = await CaseAppService.GetListAsync(new CaseGetListDto());
        Cases = res.Items;
        if (Cases.Any())
        {
            OldestRecordDate = Cases.OrderBy(c => c.DateRegistered).First().DateRegistered;
        }

    }

    private string FlattenString(string s) => s.ToLower().Replace(" ", "");

    protected bool FlatMatch(string baseString, string searchString) =>
        FlattenString(baseString).StartsWith(FlattenString(searchString));
}
public class LocationCasesDto
{
    public string Name { get; init; }
    public double Count { get; init; }
}