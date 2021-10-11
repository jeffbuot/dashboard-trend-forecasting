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
                fdPermissionGroup.AddPermission(ForecastingDashboardPermissions.Case.Default, L("Permission:Article"));
            articlesPermission.AddChild(ForecastingDashboardPermissions.Case.Create, L("Permission:Article.Create"));
            articlesPermission.AddChild(ForecastingDashboardPermissions.Case.Delete, L("Permission:Article.Delete"));
            articlesPermission.AddChild(ForecastingDashboardPermissions.Case.Edit, L("Permission:Article.Edit"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<ForecastingDashboardResource>(name);
        }
    }
}
