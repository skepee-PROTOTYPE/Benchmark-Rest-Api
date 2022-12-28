using BenchmarkRest.DynamicClass;
using BenchmarkRest.HttpHandler;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using System;
using System.Threading.Tasks;

namespace BenchmarkRest
{
    class Program
    {
        private static Parameters myParams { get; set; }

        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("");
                Console.WriteLine("parameters: ");
                Console.WriteLine(" - arg[0]: string, url to test");
                Console.WriteLine(" - arg[1]: integer, number of iterations");
                Console.WriteLine(" - arg[2]: http verb: Get (g), Post (po), Put (pu), Delete (del)(d)");
                Console.WriteLine(" - arg[3]: string, fromBody json for Post, Put and Delete.");
                Console.WriteLine(" - arg[4]: integer, id starting value for Put and Delete. If missing id starting value is equal to 1");
                return;
            }

            if (args.Length <= 1 || args.Length > 5)
            {
                Console.WriteLine("not valid input arguments. Unable to continue.");
                return;
            }

            myParams = new Parameters(args);

            var nlogger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

            using ILoggerFactory loggerFactory =
                       LoggerFactory.Create(builder =>
                           builder.AddSimpleConsole(options => {
                               options.IncludeScopes = true;
                               options.SingleLine = true;
                               options.TimestampFormat = "hh:mm:ss ";
                           }).AddConsole()
                             .AddDebug()
                             );

            ILogger<Program> logger = loggerFactory.CreateLogger<Program>();

            logger.LogInformation("Application started...");

            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) => {
                    services.AddSingleton<IMyDynamicClass, MyDynamicClass>();

                    services.AddHttpClient<IMyHttpHandler, MyHttpHandler>(client => {
                        client.BaseAddress = new Uri(myParams.url);
                    });
                    services.AddTransient<IMyHttpHandler, MyHttpHandler>();

                }).UseConsoleLifetime();

            builder.ConfigureLogging(logging => {
                logging.ClearProviders();
                logging.AddConsole();
            }).UseNLog();

            var host = builder.Build();

            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                try
                {
                    var myService = services.GetRequiredService<IMyHttpHandler>();
                    await myService.Run(myParams);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error Occured: {ex.Message}");
                }
            }
            LogManager.Shutdown();
        }
    }
}
