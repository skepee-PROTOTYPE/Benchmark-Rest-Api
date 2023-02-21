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
        private static ApiParam myParams { get; set; }

        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("");
                Console.WriteLine("BenchmarkRest <url> <numIterations> <httpVerb> <apiParams>");
                Console.WriteLine("");
                Console.WriteLine("params: ");
                Console.WriteLine(" - <url>: string, url to test");
                Console.WriteLine(" - <numIterations>: integer, number of iterations");
                Console.WriteLine(" - <httpVerb>: http verb: Get (g), Post (po), Put (pu), Delete (del)(d)");
                Console.WriteLine(" - -d {apiParams}: data api in Json format. Used to pass parameters in Body for Post, Put and Delete or in Url for Get if needed.");
                Console.WriteLine(" - -h {header}: headers in Json format. Used to pass parameters in Header.");
                return;
            }

            if (args.Length < 3)
            {
                Console.WriteLine("not valid input arguments. Unable to continue.");
                return;
            }

            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) => {
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
                    myParams = new ApiParam(args);
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
