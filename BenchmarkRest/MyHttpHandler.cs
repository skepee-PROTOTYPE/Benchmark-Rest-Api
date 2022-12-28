using BenchmarkRest;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkRest
{
    public class MyHttpHandler
    {
        private readonly ILogger _logger;
        private IHttpClientFactory _httpFactory { get; set; }
        public MyHttpHandler(ILogger<MyHttpHandler> logger, IHttpClientFactory httpFactory)
        {
            _logger = logger;
            _httpFactory = httpFactory;
        }

        public async Task Run(Parameters p)
        {
            double[] times = new double[p.numIterations];

            for (int i = 0; i < p.numIterations; i++)
            {
                HttpRequestMessage request = new HttpRequestMessage();

                request.Method = p.method;

                var obj = new MyDynamicClass(p.fromBody);

                if (request.Method == HttpMethod.Post || request.Method == HttpMethod.Put)
                {
                    request.RequestUri = new Uri(p.url);
                    string json = obj.UpdateProperties(i);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }
                else
                {
                    request.RequestUri = new Uri(p.url + "/" + obj.props.First().Value);
                }

                var stopwatch = new Stopwatch();
                stopwatch.Start();

                var client = _httpFactory.CreateClient();
                var res = await client.SendAsync(request);

                if (res.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var JsonResponse = await res.Content.ReadAsStringAsync();
                    stopwatch.Stop();
                    times[i] = stopwatch.ElapsedMilliseconds;
                    _logger.LogInformation($"{res.StatusCode} - iteration {i + 1}: {times[i]} ms at {DateTime.Now.ToString("hh.mm.ss.fff")}");
                }
                else
                {
                    _logger.LogInformation($"{res.StatusCode} - no data at {DateTime.Now.ToString("hh.mm.ss.fff")}");
                }
            }

            _logger.LogInformation($"Benchmark for {p.url}");
            _logger.LogInformation($"Results based on {p.numIterations} iterations: ");
            _logger.LogInformation($"        Avg elased time: {times.ToList().Average()} ms");
            _logger.LogInformation($"        Min elased time: {times.ToList().Min()} ms");
            _logger.LogInformation($"        Max elased time: {times.ToList().Max()} ms");
        }
    }
}
