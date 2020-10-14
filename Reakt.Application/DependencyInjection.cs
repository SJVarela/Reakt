using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Reakt.Application.Contracts.Interfaces;
using Reakt.Application.Services;

namespace Reakt.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(typeof(DependencyInjection).Assembly);
            //DI services
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IBoardService, BoardService>();
            services.AddScoped<IPostService, PostService>();
            return services;
        }
    }
}