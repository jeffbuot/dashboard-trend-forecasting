using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace SMPLX.ForecastingDashboard.MongoDB
{
    [DependsOn(
        typeof(ForecastingDashboardTestBaseModule),
        typeof(ForecastingDashboardMongoDbModule)
        )]
    public class ForecastingDashboardMongoDbTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var stringArray = ForecastingDashboardMongoDbFixture.ConnectionString.Split('?');
                        var connectionString = stringArray[0].EnsureEndsWith('/')  +
                                                   "Db_" +
                                               Guid.NewGuid().ToString("N") + "/?" + stringArray[1];

            Configure<AbpDbConnectionOptions>(options =>
            {
                options.ConnectionStrings.Default = connectionString;
            });
        }
    }
}
