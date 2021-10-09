using Volo.Abp.Modularity;

namespace SMPLX.ForecastingDashboard
{
    [DependsOn(
        typeof(ForecastingDashboardApplicationModule),
        typeof(ForecastingDashboardDomainTestModule)
        )]
    public class ForecastingDashboardApplicationTestModule : AbpModule
    {

    }
}