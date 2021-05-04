using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Serialization;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSPJQ
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
            //MVC
            services.AddControllersWithViews(config =>
            {                
            })
            // Use Newtonsoft’s Json.NET instead of System.Text.Json.            
            .AddNewtonsoftJson((options) =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            // add this to allow csp report POST
            services.AddOptions<MvcOptions>()
              .PostConfigure<IOptions<JsonOptions>, IOptions<MvcNewtonsoftJsonOptions>, ArrayPool<char>, ObjectPoolProvider, ILoggerFactory>(
                  (mvcOptions, jsonOpts, newtonJsonOpts, charPool, objectPoolProvider, loggerFactory) =>
                  {
                      var formatter = mvcOptions.InputFormatters.OfType<NewtonsoftJsonInputFormatter>().First(i => i.SupportedMediaTypes.Contains("application/json"));
                      formatter.SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/csp-report"));
                      mvcOptions.InputFormatters.RemoveType<NewtonsoftJsonInputFormatter>();
                      mvcOptions.InputFormatters.Add(formatter);
                  });

            services.AddScoped<INonceService, NonceService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<CSPMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
