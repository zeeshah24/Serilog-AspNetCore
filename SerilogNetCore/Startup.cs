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
using Serilog;

namespace SerilogNetCore
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostEnv { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment hostEnv)
        {
            Configuration = configuration;
            HostEnv = hostEnv;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
              .ReadFrom.Configuration(Configuration)
              .MinimumLevel.Information()
              // .MinimumLevel.Debug()
              .Enrich.FromLogContext()
              .WriteTo.ColoredConsole()
              //.WriteTo.File(HostEnv.WebRootPath + "\\Log\\log1.txt")
              //.WriteTo.RollingFile()
              .WriteTo.RollingFile(HostEnv.WebRootPath + @"\\Log\\log-{Date}.txt", Serilog.Events.LogEventLevel.Information, retainedFileCountLimit: 7)
              .CreateLogger();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
           //   .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
           .AddEnvironmentVariables()
           .Build();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //builder.AddUserSecrets();
            }

            app.UseMvc();
        }
    }
}
