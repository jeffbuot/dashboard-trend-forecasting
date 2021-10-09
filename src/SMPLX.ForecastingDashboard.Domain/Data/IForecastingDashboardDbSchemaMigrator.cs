using System.Threading.Tasks;

namespace SMPLX.ForecastingDashboard.Data
{
    public interface IForecastingDashboardDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}
