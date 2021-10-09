using SMPLX.ForecastingDashboard.MongoDB;
using Xunit;

namespace SMPLX.ForecastingDashboard
{
    [CollectionDefinition(ForecastingDashboardTestConsts.CollectionDefinitionName)]
    public class ForecastingDashboardDomainCollection : ForecastingDashboardMongoDbCollectionFixtureBase
    {

    }
}
