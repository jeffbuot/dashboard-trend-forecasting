using SMPLX.ForecastingDashboard.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SMPLX.ForecastingDashboard.Permissions
{
    public class ForecastingDashboardPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(ForecastingDashboardPermissions.GroupName);
            //Define your own permissions here. Example:
            //myGroup.AddPermission(ForecastingDashboardPermissions.MyPermission1, L("Permission:MyPermission1"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<ForecastingDashboardResource>(name);
        }
    }
}
