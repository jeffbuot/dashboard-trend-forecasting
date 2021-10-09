using SMPLX.ForecastingDashboard.MongoDB;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace SMPLX.ForecastingDashboard.DbMigrator
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(ForecastingDashboardMongoDbModule),
        typeof(ForecastingDashboardApplicationContractsModule)
        )]
    public class ForecastingDashboardDbMigratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
        }
    }
}
