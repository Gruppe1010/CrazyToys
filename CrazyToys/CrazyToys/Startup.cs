using CrazyToys.Data.Data;
using CrazyToys.Entities.Entities;
using CrazyToys.Interfaces;
using CrazyToys.Interfaces.EntityDbInterfaces;
using CrazyToys.Services;
using CrazyToys.Services.EntityDbServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            services.AddDbContext<Context>(options =>
            {
                options.UseSqlServer(
               _config.GetConnectionString("context"));
                options.EnableSensitiveDataLogging();
            });
            
            /*
            services.AddDbContext<Context>(options =>
            options.UseSqlServer(_config.GetConnectionString("context")),
            ServiceLifetime.Transient);
            */
            services.AddScoped<IEntityCRUD<Brand>, BrandDbService>();
            services.AddScoped<IEntityCRUD<Category>, CategoryDbService>();
            services.AddScoped<IEntityCRUD<SubCategory>, SubCategoryDbService>();
            services.AddScoped<IEntityCRUD<Colour>, ColourDbService>();
            services.AddScoped<ToyDbService>();
            services.AddScoped<IEntityCRUD<AgeGroup>, AgeGroupDbService>();
            services.AddScoped<IcecatDataService>();
            //services.AddScoped<IProductDataService, IcecatDataService>();
            services.AddScoped<IHangfireService, HangfireService>();
            
            //pragma warning disable IDE0022 // Use expression body for methods
            services.AddUmbraco(_env, _config)
                .AddBackOffice()
                .AddWebsite()
                .AddComposers()
                .Build();
            //pragma warning restore IDE0022 // Use expression body for methods
        }

        /// <summary>
        /// Configures the application.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="env">The web hosting environment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }



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


       
        }
    }
}
