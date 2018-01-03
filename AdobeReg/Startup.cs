using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using AdobeReg.Models;
using Microsoft.AspNetCore.Http.Features;
using AdobeReg.Filter;
using AdobeReg.Utility;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using AdobeReg.Formatter;
using Microsoft.AspNetCore.Http;
//using System.Net.Http.Headers;

namespace AdobeReg
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
            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 30000000;
                options.ValueLengthLimit = 30000000;
                options.KeyLengthLimit = 30000000;
            });

            services.AddDbContext<AdobeRegContext>(opt => opt.UseInMemoryDatabase("AgilOne"));
            services.AddMvc(o => o.InputFormatters.Insert(0, new RawRequestBodyFormatter()));
            services.AddSingleton<IConfiguration>(Configuration); // added to inject configuration
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ISlapCrypto, SlapCrypto>();
            services.AddTransient<IAdobeGuid, AdobeGuid>();

            services.AddMvc(mvcConfig =>
            {
                var formatter = new AdobePlainInputFormatter();
                formatter.SupportedMediaTypes.Add(
                     new MediaTypeHeaderValue("application/vnd.adobe.adept+xml"));
                mvcConfig.InputFormatters.Add(formatter);

                //var tformatter = new StringOutputFormatter();
                //formatter.SupportedMediaTypes.Add(
                //     new MediaTypeHeaderValue("application/vnd.adobe.adept+xml"));
                //mvcConfig.OutputFormatters.Add(tformatter);

                var aformatter = new AdobeOutputFormatter();
                formatter.SupportedMediaTypes.Add(
                     new MediaTypeHeaderValue("application/vnd.adobe.adept+xml"));
                mvcConfig.OutputFormatters.Add(aformatter);

            });

            //            services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
