using System.Threading.Tasks;
using SMPLX.ForecastingDashboard.Localization;
using SMPLX.ForecastingDashboard.MultiTenancy;
using Volo.Abp.Identity.Blazor;
using Volo.Abp.SettingManagement.Blazor.Menus;
using Volo.Abp.TenantManagement.Blazor.Navigation;
using Volo.Abp.UI.Navigation;

namespace SMPLX.ForecastingDashboard.Blazor.Menus
{
    public class ForecastingDashboardMenuContributor : IMenuContributor
    {
        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == StandardMenus.Main)
            {
                await ConfigureMainMenuAsync(context);
            }
        }

        private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
        {
            var administration = context.Menu.GetAdministration();
            var l = context.GetLocalizer<ForecastingDashboardResource>();

            context.Menu.Items.Insert(
                0,
                new ApplicationMenuItem(
                    ForecastingDashboardMenus.Home,
                    l["Menu:Home"],
                    "/",
                    icon: "fas fa-home",
                    order: 0
                )
            ); 
            context.Menu.Items.Insert(
                0,
                new ApplicationMenuItem(
                    ForecastingDashboardMenus.Dashboard,
                    l["Menu:Dashboard"],
                    "/dashboard",
                    icon: "fas fa-dashboard",
                    order: 1
                )
            );
            
            if (MultiTenancyConsts.IsEnabled)
            {
                administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
            }
            else
            {
                administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
            }

            administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);
            administration.SetSubItemOrder(SettingManagementMenus.GroupName, 3);
            administration.SetSubItemOrder("Data Import", 1);

            return Task.CompletedTask;
        }
    }
}
