using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using NJsonSchema;
using NSwag.AspNetCore;
using FollowerManagementApi.DataAccess;
using AutoMapper;
using Microsoft.Extensions.HealthChecks;
using FollowerManagementApi.Commands;
using FollowerManagementApi.Models;

namespace FollowerManagementApi
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
            // add DBContext classes
            var sqlConnectionString = Configuration.GetConnectionString("FollowerManagementCN");
            services.AddDbContext<FollowerManagementDBContext>(options => options.UseSqlServer(sqlConnectionString));

            services.AddSwaggerDocument();
            services.AddHealthChecks(checks => checks.WithDefaultCacheDuration(TimeSpan.FromSeconds(1)));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwaggerUi3();
            app.UseOpenApi();
            app.UseMvc();
        }
    }
}
