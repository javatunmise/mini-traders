using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Site.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using siteinfo;
using site.Repositories;
using Microsoft.CodeAnalysis;
using site.Data.Repositories;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Shared.Entities;
using Shared;
using site.ViewComponents;
using site.Configs;
using site.Helpers.Services;

namespace Site
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
            services.Configure<LatestProductsCategories>(Configuration.GetSection("LatestProductsCategories"));
            services.Configure<PaymentConfig>(Configuration.GetSection("PaymentConfiguration"));
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            PaymentConfig paymentConfig = GetPaymentConfigSection();
            services.AddHttpClient("paystack", client =>
            {
                client.BaseAddress = new Uri(paymentConfig.PaystackUrl);
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {paymentConfig.SecretKey}");
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>()
                //.AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped<IPageRepository, PageRepository>();
            services.AddScoped<site.Data.ISiteContentProvider, site.Data.CacheSiteContentProvider>();
            services.AddScoped<ISellerRepository, SellerRepository>();
            services.AddScoped<AccountRepository>();
            services.AddScoped<StoreRepository>();
            services.AddScoped<LocationRepository>();
            services.AddScoped<ProductsRepository>();
            services.AddScoped<CategoriesRepository>();
            services.AddScoped<ChatRepository>();
            services.AddScoped<PaymentRepository>();
            services.AddScoped<ICurrentDate, ServerDateTime>();
            services.AddScoped<StoreActivationHandler>();

            services.AddMvc()
                //.SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeFolder("/Profile");
                    options.Conventions.AuthorizeFolder("/Admin");
                    options.Conventions.AuthorizePage("/Sellers/Registration");
                });
        }

        private PaymentConfig GetPaymentConfigSection()
        {
            return Configuration.GetSection("PaymentConfiguration").Get<PaymentConfig>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.Use(async (context, next) => {
                if (IsLogOutUrl(context))
                {
                    context.Response.Redirect("/");
                }

                await next();
            });

            //app.Use(async (context, next) =>
            //{
            //    if (context.Response.StatusCode == 404)
            //    {
            //        context.Request.Path = "/error404";
            //        await next();
            //    }
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }

        private bool IsLogOutUrl(HttpContext context)
        {
            return context.Request.Method == "GET" && context.Request.Path.Value.EndsWith("/Identity/Account/Logout");
        }
    }
}
