using System.Collections.ObjectModel;
using MongoDB.Driver;
using SMPLX.ForecastingDashboard.Cases;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace SMPLX.ForecastingDashboard.MongoDB
{
    [ConnectionStringName("Default")]
    public class ForecastingDashboardMongoDbContext : AbpMongoDbContext
    {
        /* Add mongo collections here. Example:
         * public IMongoCollection<Question> Questions => Collection<Question>();
         */
        public IMongoCollection<Case> Questions => Collection<Case>();
        protected override void CreateModel(IMongoModelBuilder builder)
        {
            base.CreateModel(builder);

            builder.Entity<Case>(b =>
            {
                //...
            });
        }
    }
}
