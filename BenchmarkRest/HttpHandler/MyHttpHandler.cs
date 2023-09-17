using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkRest.HttpHandler
{
    public class MyHttpHandler : IMyHttpHandler
    {
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpFactory;

        public MyHttpHandler(ILogger<MyHttpHandler> logger, IHttpClientFactory httpFactory)
        {
            _logger = logger;
            _httpFactory = httpFactory;
        }

        public async Task Run(ApiParam p)
        {
            double[] times = new double[p.numIterations];

            for (int i = 0; i < p.numIterations; i++)
            {
                var request = new HttpRequestMessage();
                request.Method = p.httpVerb;

                //adding header values
                if (p.header != null)
                { 
                    foreach (var prop in p.header.props)
                        request.Headers.Add(prop.Key, prop.Value.ToString());
                }

                if (request.Method == HttpMethod.Post || request.Method == HttpMethod.Put)
                {
                    if (p.apiData.props.Any())
                    { 
                        request.RequestUri = new Uri(p.url);
                        string json = p.apiData.UpdateProperties(1);
                        _logger.LogInformation($"HttpVerb: {request.Method} - Api Data: {json}");
                        request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                    }
                }
                else
                {
                    String uri = p.url;
                    long valInt;

                    if (p.apiData != null && p.apiData.props.Any() && long.TryParse(p.apiData.props.First().Value.ToString(), out valInt))
                        uri += (p.url.Contains("?")? "" : "/") + (Convert.ToInt64(valInt + i).ToString());

                    _logger.LogInformation($"HttpVerb: {request.Method} - Api Data: {uri}");
                    request.RequestUri = new Uri(uri);
                }

                var stopwatch = new Stopwatch();
                stopwatch.Start();

                var client = _httpFactory.CreateClient();
                var res = await client.SendAsync(request);

                if (res.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    _ = await res.Content.ReadAsStringAsync();
                    stopwatch.Stop();
                    times[i] = stopwatch.ElapsedMilliseconds;
                    _logger.LogInformation($"{res.StatusCode} - iteration {i + 1}: {times[i]} ms at {DateTime.Now.ToString("hh.mm.ss.fff")}");
                }
                else
                    _logger.LogInformation($"{res.StatusCode} - no data at {DateTime.Now.ToString("hh.mm.ss.fff")}");

            }

            _logger.LogInformation($"Benchmark for {p.url}");
            _logger.LogInformation($"Results based on {p.numIterations} iterations: ");
            _logger.LogInformation($"        Avg elased time: {times.ToList().Average()} ms");
            _logger.LogInformation($"        Min elased time: {times.ToList().Min()} ms");
            _logger.LogInformation($"        Max elased time: {times.ToList().Max()} ms");
        }
    }
}
