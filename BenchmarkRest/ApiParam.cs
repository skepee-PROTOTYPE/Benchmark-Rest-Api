using System;
using System.Net.Http;

namespace BenchmarkRest
{
    public class ApiParam
    {
        public string url { get; set; }
        public int numIterations { get; set; }
        public HttpMethod httpVerb { get; set; }
        public ApiData apiData { get; set; }

        public ApiParam(string[] args)
        {
            url = args[0];
            numIterations = Convert.ToInt32(args[1]);
            httpVerb = GetMethodType(args[2].ToUpper());
            apiData = new ApiData(args[3]);
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
