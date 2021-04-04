using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CryptocurrencyTracker.Models;
using CryptocurrencyTracker.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;


namespace CryptocurrencyTracker
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
            // add context for DB EF Core
            services.AddDbContext<CryptoTrackerDbContext>(options => options.UseSqlServer(Config.DbConnectionString)); //Configuration.GetConnectionString(_connectionString)));
            services.AddControllers();
            services.AddMvc().AddNewtonsoftJson();
            services.AddControllers().AddNewtonsoftJson();
            services.AddControllersWithViews().AddNewtonsoftJson();

            // background Service for update crypto values
            services.AddHostedService<UpdateCryptoService>();
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            // setup CORS for fetch requests
            services.AddCors();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.Zero;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                //app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CryptocurrencyTracker v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //global cors policy
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                                                    //.AllowAnyOrigin()
                .AllowCredentials()); // allow credentials


            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}