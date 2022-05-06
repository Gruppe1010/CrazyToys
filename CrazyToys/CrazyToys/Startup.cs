using CrazyToys.Data.Data;
using CrazyToys.Entities.Entities;
using CrazyToys.Entities.SolrModels;
using CrazyToys.Interfaces;
using CrazyToys.Interfaces.EntityDbInterfaces;
using CrazyToys.Services;
using CrazyToys.Services.ProductDbServices;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SolrNet;
using System;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Extensions;

namespace CrazyToys.Web
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup" /> class.
        /// </summary>
        /// <param name="webHostEnvironment">The web hosting environment.</param>
        /// <param name="config">The configuration.</param>
        /// <remarks>
        /// Only a few services are possible to be injected here https://github.com/dotnet/aspnetcore/issues/9337
        /// </remarks>
        public Startup(IWebHostEnvironment webHostEnvironment, IConfiguration config)
        {
            _env = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <remarks>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        /// </remarks>
        public void ConfigureServices(IServiceCollection services)
        {
            // session
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromSeconds(3600);
            });

            // db
            services.AddDbContext<Context>(options => {
                options.UseSqlServer(
               _config.GetConnectionString("context"));
                options.EnableSensitiveDataLogging();
            });

            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
              .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
              .UseSimpleAssemblyNameTypeSerializer()
              .UseRecommendedSerializerSettings()
              .UseSqlServerStorage(_config.GetConnectionString("hangfireConnection"), new SqlServerStorageOptions
              {
                  CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                  SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                  QueuePollInterval = TimeSpan.Zero,
                  UseRecommendedIsolationLevel = true,
                  DisableGlobalLocks = true
              }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();
            services.AddScoped<IHangfireService, HangfireService>();

            // DbServices
            services.AddScoped<IEntityCRUD<Brand>, BrandDbService>();
            services.AddScoped<IEntityCRUD<Category>, CategoryDbService>();
            services.AddScoped<IEntityCRUD<SubCategory>, SubCategoryDbService>();
            services.AddScoped<IEntityCRUD<ColourGroup>, ColourGroupDbService>();
            services.AddScoped<ToyDbService>();
            services.AddScoped<ImageDbService>();
            services.AddScoped<SimpleToyDbService>();
            services.AddScoped<IEntityCRUD<AgeGroup>, AgeGroupDbService>();
            services.AddScoped< IEntityCRUD<PriceGroup>, PriceGroupDbService> ();

            services.AddScoped<IcecatDataService>();
            services.AddScoped<ISessionService, SessionService>();


            //Umbraco
            //pragma warning disable IDE0022 // Use expression body for methods
            services.AddUmbraco(_env, _config)
                .AddBackOffice()
                .AddWebsite()
                .AddComposers()
                .Build();
            //pragma warning restore IDE0022 // Use expression body for methods

            // Solr
            services.AddSolrNet<SolrToy>("http://localhost:8983/solr/toys");
            services.AddScoped<ISearchService<SolrToy>, SolrService<SolrToy, ISolrOperations<SolrToy>>>();

            // Breadcrumbs
            services.AddTransient<BreadcrumbService>();
        }

        /// <summary>
        /// Configures the application.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="env">The web hosting environment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, 
            IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // session
            app.UseSession();

            //Hangfire
            app.UseHangfireDashboard();
            app.UseRouting();
            app.UseEndpoints(endpoints => {
                endpoints.MapHangfireDashboard();
            });

            //Umbraco
            app.UseUmbraco()
                .WithMiddleware(u =>
                {
                    u.UseBackOffice();
                    u.UseWebsite();
                })
                .WithEndpoints(u =>
                {
                    u.UseInstallerEndpoints();
                    u.UseBackOfficeEndpoints();
                    u.UseWebsiteEndpoints();
                });

            string indexUrl = "https://data.Icecat.biz/export/freexml/EN/files.index.xml";
            string dailyUrl = "https://data.Icecat.biz/export/freexml/EN/daily.index.xml";

            recurringJobManager.AddOrUpdate<HangfireService>("IndexIcecat", hangfireService => hangfireService.GetProductsDataService(indexUrl, null), Cron.Never);
            recurringJobManager.AddOrUpdate<HangfireService>("DailyIcecat", hangfireService => hangfireService.GetProductsDataService(dailyUrl, null), "00 01 * * *");

            // TODO denne er bare for convenience 
            recurringJobManager.AddOrUpdate<HangfireService>("UpdateSolrDb", hangfireService => hangfireService.UpdateSolrDb(), Cron.Never);
            recurringJobManager.AddOrUpdate<HangfireService>("DeleteSolrDb", hangfireService => hangfireService.DeleteSolrDb(), Cron.Never);
        }
    }
}
