using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Cinderella.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Stripe;
using Cinderella.Data;

namespace Cinderella
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
               services.Configure<CookiePolicyOptions>(options =>
               {
                 // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                 options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
               });


               services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

              services.AddDbContext<CinderellaContext>(options =>
                      options.UseSqlServer(Configuration.GetConnectionString("CinderellaContext")));

               services.AddIdentity<ApplicationUser, ApplicationRole>()
                  .AddDefaultUI(UIFramework.Bootstrap4)
                   .AddEntityFrameworkStores<CinderellaContext>()
                   .AddDefaultTokenProviders();

               services.Configure<IdentityOptions>(options =>
               {
                    // Password settings
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequiredUniqueChars = 1;

                    // Lockout settings
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 10;
                    options.Lockout.AllowedForNewUsers = true;

                    // User settings
                    options.User.RequireUniqueEmail = true;
               });

               services.ConfigureApplicationCookie(options =>
               {
                    // options.Cookie.Name = "YourCookieName";
                    // options.Cookie.Domain=
                    // options.LoginPath = "/Account/Login";
                    // options.LogoutPath = "/Account/Logout";
                    // options.AccessDeniedPath = "/Account/AccessDenied";

                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromSeconds(500);
                    options.SlidingExpiration = true;
               });
               services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));
               services.Configure<RecaptchaSettings>(Configuration.GetSection("reCAPTCHA"));
                //Zuriel update (start)
                services.Configure<MvcOptions>(options =>
                {
                    options.Filters.Add(new RequireHttpsAttribute { Permanent = true });
                });
                //Zuriel update (end)
          }

          // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
          public void Configure(IApplicationBuilder app, IHostingEnvironment env)
          {
            StripeConfiguration.ApiKey = "sk_test_51H94fhCxH34RknCHpZXv7m5wZHqGbbfhSL0XGheU36xzg0Ucf7gJKiNTQiHZaKX1akiPepHDCcKW10CL4s6JGj0M00PtUmlX6J";
            //if (env.IsDevelopment())
            //{
            //app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            app.UseStatusCodePages("text/html", "<h1>Status code page</h1> <h2>Status Code: {0}</h2>");
               app.UseExceptionHandler("/Error");
            //}


            app.UseHttpsRedirection();
               app.UseStaticFiles();
               app.UseAuthentication();
               app.UseCookiePolicy();
            //Zuriel update (start)
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                await next();
            });
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Xss-Protection", "1");
                await next();
            });
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                await next();
            });
            //Zuriel update (end)

            app.UseMvc();
          }
     }
}
