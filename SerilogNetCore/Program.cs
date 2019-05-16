using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using SerilogNetCore;

namespace SerilogNetCore
{
    public class Program
    {
        //public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
        //   .SetBasePath(Directory.GetCurrentDirectory())
        //   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //   .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
        //   .AddEnvironmentVariables()
        //   .Build();

        public static void Main(string[] args)
        {
            //Log.Logger = new LoggerConfiguration()
            //   .ReadFrom.Configuration(Configuration)
            //   //.MinimumLevel.Information()
            //   .MinimumLevel.Debug()
            //   .Enrich.FromLogContext()
            //   .WriteTo.ColoredConsole()
            //   //.WriteTo.RollingFile()

            //   //.WriteTo.Seq("localhost:85/logs")
            //   .CreateLogger();

            try
            {
                Log.Information("Host started...");
                BuildWebHost(args).Run();
                return;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return;
            }
            finally
            {
                Log.CloseAndFlush();
            }
            //BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                //.UseConfiguration(Configuration)
                .UseSerilog()
                .Build();
    }
}
