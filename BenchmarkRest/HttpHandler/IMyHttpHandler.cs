using System.Threading.Tasks;

namespace BenchmarkRest.HttpHandler
{
    public interface IMyHttpHandler
    {
        Task Run(ApiParam p);
    }
}