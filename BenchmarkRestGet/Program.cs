using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkRestGet
{
    class Program
    {
        private static StringBuilder sb;

        static async Task Main(string[] args)
        {
            if (args.Length<=1 || args.Length >=4)
            {
                Console.WriteLine("Not enough input arguments. Unable to continue.");
                return;
            }           

            sb = new StringBuilder();
            string url = args[0];
            int numIterations = Convert.ToInt32(args[1]);

            string hasLog = "";
            if (args.Length==2)
                hasLog = "Y";
            else
                hasLog = args[2].Trim().ToUpper();

            string logFile = "";

            double[] times = new double[numIterations];
            AddLog($"Url: {url}");

            for (int i = 0; i < numIterations; i++)
            {
                var client = new HttpClient();
                var req = new HttpRequestMessage(HttpMethod.Get, url);

                var stopwatch = new Stopwatch();
                stopwatch.Start();

                var res = await client.SendAsync(req);

                if (res.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jsonResponse = await res.Content.ReadAsStringAsync();
                    if (jsonResponse.Length > 0)
                    {
                        stopwatch.Stop();
                        times[i] = stopwatch.ElapsedMilliseconds;

                        AddLog($"{res.StatusCode} - iteration {i + 1}: {times[i]} ms at {DateTime.Now.ToString("HH.mm.ss.fff")}");
                    }
                }
                else 
                {
                    AddLog($"{res.StatusCode} - no data at {DateTime.Now.ToString("HH.mm.ss.fff")}");
                }
            }
            AddLog("");
            AddLog("---------------------------------------------------------------------------------------------");
            AddLog($"Benchmark for {url}");
            AddLog($"Results based on {numIterations} iterations: ");
            AddLog($"        Avg elased time: {times.ToList().Average()} ms");
            AddLog($"        Min elased time: {times.ToList().Min()} ms");
            AddLog($"        Max elased time: {times.ToList().Max()} ms");
            AddLog("---------------------------------------------------------------------------------------------");

            if (hasLog == "Y")
            {
                logFile = $"BenchmarkGetRest_{DateTime.Now.ToString("dd.MM.yyyy_HH.mm.ss")}.txt";
                File.AppendAllText(logFile,sb.ToString());
            }
        }

        private static void AddLog(string logItem)
        {
            sb.AppendLine(logItem);
            Console.WriteLine(logItem);
        }

    }
}
