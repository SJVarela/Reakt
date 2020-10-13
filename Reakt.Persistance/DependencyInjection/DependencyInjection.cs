using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Reakt.Application.Persistence;
using Reakt.Persistance.DataAccess;

namespace Reakt.Persistance.DependencyInjection
{
    public static class DependencyInjection
    {
        //For testing and debugging
        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder =>
             {
                 builder
                     .AddFilter((category, level) =>
                         category == DbLoggerCategory.Database.Command.Name
                         && level == LogLevel.Information)
                     .AddConsole();
             });

        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ReaktDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ReaktDbConnection"))
                .UseLoggerFactory(MyLoggerFactory));
            services.AddScoped<IReaktDbContext>(provider => provider.GetService<ReaktDbContext>());

            services.BuildServiceProvider().GetService<ReaktDbContext>().Database.EnsureCreated();

            return services;
        }
    }
}