using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core;

public class GenericContractResolver<T> : DefaultContractResolver
{

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);
        if (property.UnderlyingName == nameof(Result<T>.Payload))
        {
            foreach (var attribute in Attribute.GetCustomAttributes(typeof(T)))
            {
                if (attribute is JsonObjectAttribute jobject)
                {
                    property.PropertyName = jobject.Id;
                }
            }
        }
        return property;
    }
}