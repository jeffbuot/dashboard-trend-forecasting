using System.Collections.Generic;
using System.Threading.Tasks;
using SMPLX.ForecastingDashboard.Permissions;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;

namespace SMPLX.ForecastingDashboard
{
    public class DefaultDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IPermissionManager _permissionManager;
        private readonly IPermissionDataSeeder _permissionDataSeeder;

        public DefaultDataSeedContributor(IPermissionManager permissionManager,
            IPermissionDataSeeder permissionDataSeeder)
        {
            _permissionManager = permissionManager;
            _permissionDataSeeder = permissionDataSeeder;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            //set admin default permissions
            var permisssions = new List<string>
            {
                ForecastingDashboardPermissions.Case.Default,
                ForecastingDashboardPermissions.Case.Create,
                ForecastingDashboardPermissions.Case.Delete,
                ForecastingDashboardPermissions.Case.Edit,
            };

                await _permissionDataSeeder.SeedAsync(RolePermissionValueProvider.ProviderName, "admin", permisssions,
                context?.TenantId);
        }
    }
}