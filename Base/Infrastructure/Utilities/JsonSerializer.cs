using System;
using Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace Infrastructure.Utilities
{
    public class JsonSerializer : IMessageSerializer
    {
        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
    }
}
