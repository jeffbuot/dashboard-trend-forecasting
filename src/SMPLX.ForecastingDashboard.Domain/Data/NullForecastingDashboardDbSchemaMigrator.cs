using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace SMPLX.ForecastingDashboard.Data
{
    /* This is used if database provider does't define
     * IForecastingDashboardDbSchemaMigrator implementation.
     */
    public class NullForecastingDashboardDbSchemaMigrator : IForecastingDashboardDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}