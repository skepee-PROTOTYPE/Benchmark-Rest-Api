using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHttpClient();
                    services.AddTransient<MyHttpHandler>();

                }).UseConsoleLifetime();

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
        }
    }
}
