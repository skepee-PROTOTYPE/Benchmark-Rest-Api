using System;
using System.Net.Http;

namespace BenchmarkRest
{
    public class Parameters
    {
        public string url { get; set; }
        public int numIterations { get; set; }
        public HttpMethod method { get; set; }
        public string fromBody { get; set; }
        public int startingIteration { get; set; }

        public Parameters(string[] args)
        {
            url = args[0];
            numIterations = Convert.ToInt32(args[1]);
            method = GetMethodType(args[2].ToUpper());
            fromBody = args[3].ToString();
            if (args.Length == 5)
                startingIteration = Convert.ToInt32(args[4]);
        }

        private HttpMethod GetMethodType(string method)
        {
            switch (method)
            {
                case "GET":
                case "G":
                    return HttpMethod.Get;

                case "POST":
                case "PO":
                    return HttpMethod.Post;

                case "PUT":
                case "PU":
                    return HttpMethod.Put;

                case "DELETE":
                case "DEL":
                case "D":
                    return HttpMethod.Delete;

                default:
                    return null;
            }
        }
    }
}
