using SMPLX.ForecastingDashboard.MongoDB;
using Volo.Abp.Modularity;

namespace SMPLX.ForecastingDashboard
{
    [DependsOn(
        typeof(ForecastingDashboardMongoDbTestModule)
        )]
    public class ForecastingDashboardDomainTestModule : AbpModule
    {

    }
}