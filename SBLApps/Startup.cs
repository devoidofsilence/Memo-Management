//using Elsa;
//using Elsa.Activities.UserTask.Extensions;
//using Elsa.Client.Extensions;
//using Elsa.Persistence.EntityFramework.Core.Extensions;
//using Elsa.Persistence.EntityFramework.SqlServer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SBLApps.Data;
using SBLApps.Services;

namespace SBLApps
{
    public class Startup
    {
        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        private IWebHostEnvironment Environment { get; }
        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            // For Dashboard.
            services.AddControllersWithViews();

            services.AddRazorPages();

            services.AddDbContext<MemoAppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SBLAppsDbConnection")));

            //Dependency Registration
            services.AddScoped<DbHelper>();
            services.AddScoped<CBSHelperService>();
            services.AddScoped<CommonService>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseStaticFiles() // For Dashboard.
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Account}/{action=Login}/{id?}"
                    );
                });

        }
    }
}