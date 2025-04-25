using CRS.ChannelManager.Domain;
using CRS.ChannelManager.Library.BaseInterface;
using Microsoft.EntityFrameworkCore;

namespace CRS.ChannelManager.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void AddAutoMigration(this IApplicationBuilder applicationBuilder)
        {
            try
            {
                using (var scope = applicationBuilder.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
                {
                    var dbContext = (CRSChannelManagerContext)scope.ServiceProvider.GetRequiredService<IDbContext>();
                    if (dbContext.Database.GetPendingMigrations().Count() > 0)
                    {
                        dbContext.Database.Migrate();
                    }
                    SeedData.Excute(dbContext);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
