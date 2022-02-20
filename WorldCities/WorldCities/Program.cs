using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
            // SendGrid implementation test
            Execute().Wait();


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


        static async Task Execute()
        {
            var apiKey = Environment.GetEnvironmentVariable("WorldCities");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("sfi@abv.bg", "Example User");
            var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress("sfi@abv.bg", "Example User");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }

    }
}
