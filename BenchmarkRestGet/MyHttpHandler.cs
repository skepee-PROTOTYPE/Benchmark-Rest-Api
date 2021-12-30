using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BenchmarkRestGet
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
            double[] times= new double[p.numIterations];

            HttpRequestMessage request = new HttpRequestMessage();
            object fromBodyObj;

            for (int i = 0; i < p.numIterations; i++)
            {
                switch (Enum.Parse(typeof(Method), p.method))
                {
                    case Method.Get:
                        request = new HttpRequestMessage(HttpMethod.Get, p.url);
                        break;

                    case Method.Post:
                        request = new HttpRequestMessage(HttpMethod.Post, p.url);
                        fromBodyObj = new { PortfolioCode = $"xxxCode{i.ToString()}", PortfolioName = $"xxxName{i.ToString()}", PortfolioStatus = $"xxxStatus{i.ToString()}", PortfolioType = $"xxxType{i.ToString()}" };
                        request.Content = JsonContent.Create(fromBodyObj);
                        break;

                    case Method.Put:
                        request = new HttpRequestMessage(HttpMethod.Put, p.url);
                        fromBodyObj = new { PortfolioId = p.startingIteration + i, PortfolioCode = $"xxxCode{i.ToString()}", PortfolioName = $"xxxName{i.ToString()}", PortfolioStatus = $"xxxStatus{i.ToString()}", PortfolioType = $"xxxType{i.ToString()}" };
                        request.Content = JsonContent.Create(fromBodyObj);
                        break;

                    case Method.Delete:
                        request = new HttpRequestMessage(HttpMethod.Delete, p.url);
                        fromBodyObj = p.startingIteration + i;
                        request.Content = JsonContent.Create(fromBodyObj);
                        break;
                }

                var stopwatch = new Stopwatch();
                stopwatch.Start();

                var client = _httpFactory.CreateClient();
                var res = await client.SendAsync(request);

                if(res.StatusCode== System.Net.HttpStatusCode.OK)
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
        }
    }
}
