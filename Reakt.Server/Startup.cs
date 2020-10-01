using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Reakt.Application.Contracts.Interfaces;
using Reakt.Application.Services;
using Reakt.Persistance.DataAccess;
using Reakt.Persistance.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reakt.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddLogging(conf => conf.AddConsole());
            services.AddPersistence(Configuration);
            services.AddAutoMapper(typeof(Startup));
            services.AddControllers();
            //For testing!
            services.AddCors(opt => opt.AddDefaultPolicy(builder => builder.AllowAnyOrigin()));
            /*
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                    .WithOrigins(Configuration.GetValue<string>("URLWeb"))
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
            */

            //DI services
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IBoardService, BoardService>();

            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo() { Title = "Reakt API" }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Reakt API v1");
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
    }
}
