using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace BenchmarkRest.DynamicClass
{
    public class MyDynamicClass : IMyDynamicClass
    {
        public Dictionary<string, object> props { get; set; }

        public MyDynamicClass(string json)
        {
            props = new Dictionary<string, object>();
            var obj = JsonConvert.DeserializeObject<ExpandoObject>(json);

            foreach (var prop in obj)
                props.Add(prop.Key, prop.Value);
        }

        public string UpdateProperties(int i)
        {
            long valInt;

            foreach (var prop in props)
            {
                if (long.TryParse(prop.Value.ToString(), out valInt))
                    props[prop.Key] = Convert.ToInt64(valInt + i);
                else
                    props[prop.Key] = prop.Value.ToString().Substring(0, prop.Value.ToString().Length - 1) + i.ToString();
            }

            return System.Text.Json.JsonSerializer.Serialize(props);
        }
    }
}
