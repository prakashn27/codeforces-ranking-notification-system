﻿using System;
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
using UserManagementApi.DataAccess;
using AutoMapper;
using Microsoft.Extensions.HealthChecks;
using UserManagementApi.Commands;
using UserManagementApi.Models;

namespace UserManagementApi
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
            var sqlConnectionString = Configuration.GetConnectionString("UserManagementCN");
            
            services.AddDbContext<UserManagementDBContext>(options => options.UseSqlServer(sqlConnectionString));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSwaggerDocument();
            services.AddHealthChecks(checks => checks.WithDefaultCacheDuration(TimeSpan.FromSeconds(1)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // if (env.IsDevelopment())
            // {
            //     app.UseDeveloperExceptionPage();
            // }
            // else
            // {
            //     // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //     app.UseHsts();
            // }
            
            app.UseSwaggerUi3();
            app.UseOpenApi();
            app.UseMvc();

            // // setup automapper
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<RegisterUser, User>();
                cfg.CreateMap<User, RegisterUser>()
                    .ForCtorParam("messageId", opt => opt.MapFrom(c => Guid.NewGuid()));
            });
            
        }
    }
}
