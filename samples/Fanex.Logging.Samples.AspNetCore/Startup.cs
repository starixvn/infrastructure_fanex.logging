using System.Collections.Generic;
using Fanex.Data;
using Fanex.Logging.Extensions.AspNetCore;
using Fanex.Logging.Extensions.Database;
using Fanex.Logging.RabbitMQ;
using Fanex.Logging.Sentry;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sentry;

namespace Fanex.Logging.Samples.AspNetCore
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
            LogManager
                   .SetDefaultLogCategory("Fanex-Logging-Test-Web-new")
                   //.Use(new EntlibLogging(new EntLibConfig(
                   //    "FormatName:DIRECT=http://10.18.200.103:808/msmq/Private$/entlib",
                   //    @"~\Logs\")));
                   .Use(new List<ILogEngine>
                   {
                       new RabbitMQLogging("host=10.18.200.103;virtualHost=/Fanex_Logging;username=fanexlogging;password=YKQrMtla7oYZGlP8apPtSw=="),
                       new SentryLogging(new SentryOptions
                       {
                           Dsn = new Dsn("http://21a8f21f7c5a4cc1b9c79bb68fb0ebb9@sentry.nexdev.net/3"),
                           Debug = true,
                           Release = "Release-2018-10-25",
                           //AttachStacktrace = true,
                           SendDefaultPii = true
                       })
                   });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddSessionStateTempDataProvider()
                .AddRazorPagesOptions(o =>
                {
                    o.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
                });
            services.AddSingleton(LogService.LoggerInstance);

            DbSettingProviderManager
                .StartNewSession()
                .UseConnectionStrings(new Dictionary<string, string>
                {
                    { "DBLOG.bodb_CustomerLog.bodbAdminNet", "packet size=4096; Data Source=10.18.200.232;Initial Catalog=bodb_OER;integrated security=false; User Id=bodbAdminNet;Password=i9hNnaZltx04XF6oTNOKTnvvtLC/zfxr;" }
                })
                .UseDefaultDbSettingProvider(resourcePath: "~/App_Data/")
                .Run();

            ExceptionHandler
                // To use custom config
                .UseConfig(new LoggingConfiguration()
                // To ignore exceptions
                //.IgnoreException<DivideByZeroException>()
                // To hide sensitive information
                .HideSensitiveInfo(
                    new List<string>
                    {
                        "(?:password=)(?<paramValue>[^&|^\\s]+)",
                        "(\"password\":)(?<paramValue>[^,|^\r]+)",
                        "(?:seccode=)(?<paramValue>[^&|^\\s]+)",
                        "(\"seccode\":)(?<paramValue>[^,|^\r]+)",
                        "(?:securitycode=)(?<paramValue>[^&|^\\s]+)",
                        "(\"securitycode\":)(?<paramValue>[^,|^\r]+)"
                    })
                // To Enable Log Http Header
                .GetHttpHeaderInfo()
                // To Enable Log Http Session
                .GetHttpSessionInfo())
                .UseGetDbExceptionInfo(DatabaseInfoGetters.GetDbExceptionInfo);

            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseMvc();
        }
    }
}