using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkRestGet
{
    public class Parameters
    {
        public string url { get; set; }
        public int numIterations { get; set; }
        public string method { get; set; }
        public int startingIteration { get; set; }

        public Parameters(string[] args)
        {
            url = args[0];
            numIterations = Convert.ToInt32(args[1]);
            method = GetMethodType(args[2].ToUpper());
            if (args.Length==4) 
                startingIteration = Convert.ToInt32(args[3]);
        }

        private string GetMethodType(string method)
        {
            string res= string.Empty;
            switch (method)
            {
                case "GET":
                case"G":
                    res= "Get";
                    break;

                case "POST":
                case "PO":
                    res = "Post";
                    break;

                case "PUT":
                case "PU":
                    res = "Put";
                    break;

                case "DELETE":
                case "DEL":
                case "D":
                    res = "Delete";
                    break;            }

            return res;        
        }

    }
}
