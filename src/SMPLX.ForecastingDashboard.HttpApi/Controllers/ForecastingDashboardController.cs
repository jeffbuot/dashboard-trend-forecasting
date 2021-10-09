using SMPLX.ForecastingDashboard.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SMPLX.ForecastingDashboard.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class ForecastingDashboardController : AbpController
    {
        protected ForecastingDashboardController()
        {
            LocalizationResource = typeof(ForecastingDashboardResource);
        }
    }
}