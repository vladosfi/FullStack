using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using Serilog.Events;



using SendGrid;
using SendGrid.Helpers.Mail;

namespace WorldCities
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json",
                    optional: false,
                    reloadOnChange: true)
                    .AddJsonFile(string.Format("appsettings.{0}.json", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"),
                    optional: true,
                    reloadOnChange: true)
                    .AddUserSecrets<Startup>(optional: true, reloadOnChange: true)
                    .Build();
            Log.Logger = new LoggerConfiguration()
                    .WriteTo.MSSqlServer(
                    connectionString:
                    configuration.GetConnectionString("DefaultConnection"),
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        TableName = "LogEvents",
                        AutoCreateSqlTable = true
                    })
                    .WriteTo.Console()
                    .CreateLogger();
            CreateHostBuilder(args).UseSerilog().Build().Run();


        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
