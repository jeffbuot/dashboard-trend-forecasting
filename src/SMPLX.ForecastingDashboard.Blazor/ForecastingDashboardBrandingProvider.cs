using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace SMPLX.ForecastingDashboard.Blazor
{
    [Dependency(ReplaceServices = true)]
    public class ForecastingDashboardBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "ForecastingDashboard";
    }
}
