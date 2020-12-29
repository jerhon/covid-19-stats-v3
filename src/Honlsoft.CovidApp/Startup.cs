using System;
using System.Linq;
using Honlsoft.CovidApp.CovidTrackingProject;
using Honlsoft.CovidApp.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            services.AddSingleton<ICovidTrackingDataService, CovidTrackingDataService>();
            
            // Services for initialization
            services.AddHostedService<CovidDataContextInitializationService>();
            services.AddHostedService<CovidTrackingProjectImportService>();
            
            services.AddOpenApiDocument(d =>
            {
                d.DocumentName = "hs-covid-19-v1";
                d.Title = "Honlsoft COVID-19 API";
                d.Description = "Data from the COVID-19 Tracking Project aggregated in different ways to support API access.";
                d.Version = "v1";
            });

            // We'll create a single database connect, that will be used while the application is running.
            // it will be maintained in memory.  Will have to watch memory usage to see how it handles it.
            var database = SqlLiteUtils.CreateInMemoryDatabase();

            Action<DbContextOptionsBuilder> dbOptions = (opts) =>
            {
                opts.UseSqlite(database);
            };
            
            // The DB context needs to be set as a singleton, otherwise EF core get's confused between the two following calls.
            services.AddDbContext<CovidDataContext>(dbOptions, optionsLifetime: ServiceLifetime.Singleton);
            services.AddDbContextFactory<CovidDataContext>(dbOptions);

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
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseOpenApi();
            
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
    }
}
