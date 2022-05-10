using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Restaurant.Server.Models;

namespace Restaurant.Server
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // Configure Services
        public void ConfigureServices(IServiceCollection services)
        {
            //Database context
            services.AddDbContext<RestaurantContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("RestaurantDbContext")));

            //Identity
            services.AddIdentity<AdminUser, IdentityRole<int>>()
                .AddEntityFrameworkStores<RestaurantContext>()
                .AddDefaultTokenProviders();

            //Password policy
            services.Configure<IdentityOptions>(o =>
            {
                o.Password.RequireDigit = false;
                o.Password.RequiredLength = 4;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
            });

            //Cookie policy
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/api/login";
                options.ExpireTimeSpan = TimeSpan.FromSeconds(1500);
            });

            //Session
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.Name = ".Restaurant.Session";
                options.IdleTimeout = TimeSpan.FromMinutes(1);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            //MVC
            services.AddMvc()
                .AddSessionStateTempDataProvider();
        }

        // Configure Pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider isp)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            //app.UseCookiePolicy();

            app.UseMvc();

            var context = isp.GetRequiredService<RestaurantContext>();
            var userManager = isp.GetRequiredService<UserManager<AdminUser>>();
            var roleManager = isp.GetRequiredService<RoleManager<IdentityRole<int>>>();
            DBInitializer.Initialize(context, userManager, roleManager);
        }
    }
}
