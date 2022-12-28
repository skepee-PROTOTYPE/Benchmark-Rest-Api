using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace BenchmarkRest
{
    internal class MyDynamicClass
    {
        internal Dictionary<string, object> props { get; set; }

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
            string valString = string.Empty;

            foreach (var prop in props)
            {
                if (Int64.TryParse(prop.Value.ToString(), out valInt))
                    props[prop.Key] = Convert.ToInt64(valInt + i);
                else
                    props[prop.Key] = prop.Value.ToString().Substring(0, prop.Value.ToString().Length - 1) + i.ToString();
            }

            string jsonString = System.Text.Json.JsonSerializer.Serialize(props);
            return jsonString;
        }
    }
}
