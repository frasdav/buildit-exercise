using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using Wipro.WebCrawler.App;
using Wipro.WebCrawler.App.Helpers;
using Wipro.WebCrawler.Interfaces;
using Wipro.WebCrawler.Interfaces.Helpers;
using IUrlHelper = Wipro.WebCrawler.Interfaces.Helpers.IUrlHelper;

namespace Wipro.WebCrawler.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IRequestHelper, RequestHelper>();
            services.AddTransient<IUrlHelper, UrlHelper>();
            services.AddTransient<IWebCrawler, StandardWebCrawler>();
            services.AddSingleton<HttpClient>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
