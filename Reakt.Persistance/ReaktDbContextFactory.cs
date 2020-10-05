using Microsoft.EntityFrameworkCore;
using Reakt.Persistance.DataAccess;

namespace Reakt.Persistance
{
    public class ReaktDbContextFactory : DesignTimeDbContextFactory<ReaktDbContext>
    {
        protected override ReaktDbContext CreateNewInstance(DbContextOptions<ReaktDbContext> options)
        {
            return new ReaktDbContext(options);
        }
    }
}