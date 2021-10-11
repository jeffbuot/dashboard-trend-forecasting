using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SMPLX.ForecastingDashboard.Localization;
using SMPLX.ForecastingDashboard.MultiTenancy;
using SMPLX.ForecastingDashboard.Permissions;
using Volo.Abp.Identity.Blazor;
using Volo.Abp.SettingManagement.Blazor.Menus;
using Volo.Abp.TenantManagement.Blazor.Navigation;
using Volo.Abp.UI.Navigation;

namespace SMPLX.ForecastingDashboard.Blazor.Menus
{
    public class ForecastingDashboardMenuContributor : IMenuContributor
    {
        public ForecastingDashboardMenuContributor()
        {
        }

        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == StandardMenus.Main)
            {
                await ConfigureMainMenuAsync(context);
            }
        }

        private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
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
                1,
                new ApplicationMenuItem(
                    ForecastingDashboardMenus.Dashboard,
                    l["Menu:Dashboard"],
                    "/dashboard",
                    icon: "fas fa-chart-line",
                    order: 1
                )
            );

            var casesMenu = new ApplicationMenuItem(
                ForecastingDashboardMenus.Case.Default,
                l["Menu:Cases"],
                icon: "fas fa-database",
                order: 2
            );
            casesMenu.AddItem(new ApplicationMenuItem(
                ForecastingDashboardMenus.Case.Query,
                l["Menu:Cases.Query"],
                "/cases",
                icon: "fas fa-table"
            ));

            if (await context.AuthorizationService.IsGrantedAsync(ForecastingDashboardPermissions.Case.Create))
            {
                casesMenu.AddItem(new ApplicationMenuItem(
                    ForecastingDashboardMenus.Case.Import,
                    l["Menu:Cases.Import"],
                    "/cases/import",
                    icon: "fas fa-upload"
                ));
            }

            context.Menu.Items.Insert(
                1, casesMenu
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

            // return Task.CompletedTask;
        }
    }
}