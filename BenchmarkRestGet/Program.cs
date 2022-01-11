using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using NLog;
using NLog.Web;
using System;
using System.Threading.Tasks;

namespace BenchmarkRestGet
{
    enum Method
    { 
        Get,
        Post,
        Put,
        Patch,
        Delete
    }

    class Program
    {
        private static Parameters myParams { get; set; }

        static async Task Main(string[] args)
        {
            if(args.Length <= 1 || args.Length > 4)
            {
                Console.WriteLine("not enough input arguments. Unable to continue.");
                return;
            }

            myParams = new Parameters(args);

            var nlogger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

            using ILoggerFactory loggerFactory =
                       LoggerFactory.Create(builder =>
                           builder.AddSimpleConsole(options =>
                           {
                               options.IncludeScopes = true;
                               options.SingleLine = true;
                               options.TimestampFormat = "hh:mm:ss ";
                           }).AddConsole()
                             .AddDebug()
                             );

            ILogger<Program> logger = loggerFactory.CreateLogger<Program>();

            logger.LogInformation("Application started...");

            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHttpClient();
                    services.AddTransient<MyHttpHandler>();

                }).UseConsoleLifetime();

            builder.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                //logging.SetMinimumLevel(LogLevel.Trace);
            }).UseNLog();

            var host = builder.Build();

            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                try
                {
                    var myService = services.GetRequiredService<MyHttpHandler>();
                    await myService.Run(myParams);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error Occured: {ex.Message}");
                }
            }
            NLog.LogManager.Shutdown();
        }
    }
}
