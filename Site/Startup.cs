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
using site.Data.Repositories;
using System.Threading.Tasks;
using System;
using Shared;
using site.ViewComponents;
using site.Configs;
using site.Helpers.Services;
using site.Helpers;
using Microsoft.AspNetCore.Identity.UI.Services;
using site.Data;

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
            services.Configure<EmailSetting>(Configuration.GetSection("EmailSetting"));            

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
            services.AddScoped<IEmailSender, EmailSender>();

            services.AddControllersWithViews()
                    .AddRazorRuntimeCompilation();

            services.AddRazorPages(options =>
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration, ApplicationDbContext dbContext)
        {
            dbContext.Database.Migrate();
            Program.SeedLocations(dbContext);

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
            app.UseCookiePolicy();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapWhen(context => IsLogOutUrl(context), app =>
            {
                app.Run(async context => { context.Response.Redirect("/"); await Task.CompletedTask; });
            });

            app.MapWhen(context => AdminPagesButNotAdminUser(context), HandleUnauthorisedUserForAdminPages);
            app.MapWhen(context => LastSegmentIs(context, "/products"), app => RedirectTo(app, "/search"));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });

            
        }

        class RD
        {
            async Task Invoke(RequestDelegate d)
            {
                await Task.CompletedTask;
            }
        }

        private bool LastSegmentIs(HttpContext context, string path)
        {
            return context.Request.Path.Value.EndsWith(path, StringComparison.OrdinalIgnoreCase);
        }

        private void RedirectTo(IApplicationBuilder app, string path)
        {
            app.Run(async context => {
                context.Response.Redirect(path);
                await Task.CompletedTask;
            });
        }

        private void HandleUnauthorisedUserForAdminPages(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                context.Response.Redirect("/");
                await Task.CompletedTask;
            });
        }

        private bool AdminPagesButNotAdminUser(HttpContext context)
        {
            return context.Request.Path.StartsWithSegments("/admin", StringComparison.OrdinalIgnoreCase)
                && !StoreUtil.IsAdmin(context.User.Identity.Name, Configuration);
        }

        private bool IsLogOutUrl(HttpContext context)
        {
            return context.Request.Method == "GET" && context.Request.Path.Value.EndsWith("/Identity/Account/Logout");
        }
    }
}
