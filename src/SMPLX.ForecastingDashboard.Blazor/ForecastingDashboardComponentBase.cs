using SMPLX.ForecastingDashboard.Localization;
using Volo.Abp.AspNetCore.Components;

namespace SMPLX.ForecastingDashboard.Blazor
{
    public abstract class ForecastingDashboardComponentBase : AbpComponentBase
    {
        protected ForecastingDashboardComponentBase()
        {
            LocalizationResource = typeof(ForecastingDashboardResource);
        }
        
    }
}
