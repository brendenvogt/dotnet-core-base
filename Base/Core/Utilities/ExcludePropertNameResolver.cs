using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Core.Utilities
{
    public class ExcludePropertyNameResolver : CamelCasePropertyNamesContractResolver
    {
        readonly string[] _propertyNamesToExclude;

        public ExcludePropertyNameResolver(string[] propertyNamesToExclude)
        {
            _propertyNamesToExclude = propertyNamesToExclude;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properties = base.CreateProperties(type, memberSerialization);
            properties = properties.Where(p => !_propertyNamesToExclude.Any(a => string.Equals(a, p.PropertyName, StringComparison.OrdinalIgnoreCase))).ToList();
            return properties;
        }
    }
}
