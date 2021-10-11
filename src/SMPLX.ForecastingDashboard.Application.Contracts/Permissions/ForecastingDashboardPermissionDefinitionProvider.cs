using SMPLX.ForecastingDashboard.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SMPLX.ForecastingDashboard.Permissions
{
    public class ForecastingDashboardPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var fdPermissionGroup = context.AddGroup(ForecastingDashboardPermissions.GroupName);
            
            var articlesPermission =
                fdPermissionGroup.AddPermission(ForecastingDashboardPermissions.Case.Default, L("Permission:Case"));
            articlesPermission.AddChild(ForecastingDashboardPermissions.Case.Create, L("Permission:Case.Create"));
            articlesPermission.AddChild(ForecastingDashboardPermissions.Case.Delete, L("Permission:Case.Delete"));
            articlesPermission.AddChild(ForecastingDashboardPermissions.Case.Edit, L("Permission:Case.Edit"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<ForecastingDashboardResource>(name);
        }
    }
}
