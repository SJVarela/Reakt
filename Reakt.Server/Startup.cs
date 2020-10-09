using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Reakt.Application.Contracts.Interfaces;
using Reakt.Application.Services;
using Reakt.Persistance.DependencyInjection;
using System;
using System.IO;

namespace Reakt.Server
{
    /// <summary>
    /// Class to for server bootstrapping
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Reakt API");
            });
            app.UseHttpsRedirection();
            //For testing only
            app.UseCors();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(conf => conf.AddConsole());
            services.AddPersistence(Configuration);
            services.AddAutoMapper(typeof(Startup));

            services.AddControllers()
                .AddNewtonsoftJson();
            //For testing!
            services.AddCors(opt => opt.AddDefaultPolicy(builder => builder.AllowAnyOrigin()));

            //DI services
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IBoardService, BoardService>();
            services.AddScoped<IPostService, PostService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Reakt API", Version = "v1" });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{typeof(Startup).Assembly.GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }
    }
}