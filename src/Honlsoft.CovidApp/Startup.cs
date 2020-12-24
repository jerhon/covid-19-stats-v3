using System;
using System.Linq;
using Honlsoft.CovidApp.CovidTrackingProject;
using Honlsoft.CovidApp.Data;
using Honlsoft.CovidApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Honlsoft.CovidApp
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
            services.AddControllersWithViews();
            services.AddHttpClient();
            services.AddTransient<CovidTrackingProjectImport>();
            services.AddHostedService<CovidTrackingProjectImportService>();
            services.AddSingleton<ICovidTrackingDataService, CovidTrackingDataService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Honlsoft Covid Tracking API", Version = "v1"});
            });

            Action<DbContextOptionsBuilder> dbContextOptsBuilder = (opts) =>
            {
                opts.UseInMemoryDatabase("CovidDataContext");
            };
            
            services.AddDbContext<CovidDataContext>((opts) => opts.UseInMemoryDatabase("CovidDataContext"), optionsLifetime: ServiceLifetime.Singleton);
            services.AddDbContextFactory<CovidDataContext>((opts) => opts.UseInMemoryDatabase("CovidDataContext"));

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            SetupDatabase(app);
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseSwagger();
            
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "api/{controller}/{action=Index}/{id?}");
            });

            string[] ignorePaths = {"/api", "/swagger"};
            app.MapWhen((context) => ignorePaths.All((p) => !context.Request.Path.StartsWithSegments(p)), builder =>
            {
                builder.UseSpa(spa =>
                {
                    spa.Options.SourcePath = "ClientApp";

                    if (env.IsDevelopment())
                    {
                        spa.UseReactDevelopmentServer(npmScript: "start");
                    }
                });
            });
        }

        public static void SetupDatabase(IApplicationBuilder builder)
        {
            var fact = builder.ApplicationServices.GetService<IDbContextFactory<CovidDataContext>>();
            using var context = fact.CreateDbContext();

            // Any starting data should be imported here.
            context.States.ImportData("State.json");
            context.SaveChanges();
        }
    }
}
