using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Reakt.Persistance.DataAccess;
using Reakt.Application.Persistence;

namespace Reakt.Persistance.DependencyInjection
{
    public static class DependencyInjection
    {        
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ReaktDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ReaktDbConnection")));

            services.AddScoped<IReaktDbContext>(provider => provider.GetService<ReaktDbContext>());

            return services;
        }
        
    }
}
