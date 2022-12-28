using System.Collections.Generic;

namespace BenchmarkRest.DynamicClass
{
    public interface IMyDynamicClass
    {
        Dictionary<string, object> props { get; set; }

        string UpdateProperties(int i);
    }
}