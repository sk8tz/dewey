﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Reflection;

namespace Dewey.Net.Mvc.Json
{
    public class JsonPropertyNameConverter : CamelCasePropertyNamesContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var res = base.CreateProperty(member, memberSerialization);

            var attrs = member.GetCustomAttributes(typeof(JsonPropertyAttribute), true);

            if (attrs.Any()) {
                var attr = (attrs[0] as JsonPropertyAttribute);
                if (res.PropertyName != null) {
                    res.PropertyName = attr.PropertyName;
                }
            }

            return res;
        }
    }
}