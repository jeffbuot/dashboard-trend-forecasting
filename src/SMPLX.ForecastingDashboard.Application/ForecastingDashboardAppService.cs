using System;
using System.Collections.Generic;
using System.Text;
using SMPLX.ForecastingDashboard.Localization;
using Volo.Abp.Application.Services;

namespace SMPLX.ForecastingDashboard
{
    /* Inherit your application services from this class.
     */
    public abstract class ForecastingDashboardAppService : ApplicationService
    {
        protected ForecastingDashboardAppService()
        {
            LocalizationResource = typeof(ForecastingDashboardResource);
        }
    }
}
