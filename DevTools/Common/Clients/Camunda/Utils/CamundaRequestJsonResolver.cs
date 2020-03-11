using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using VXDesign.Store.DevTools.Common.Core.HTTP;

namespace VXDesign.Store.DevTools.Common.Clients.Camunda.Utils
{
    internal class CamundaRequestJsonResolver : DefaultContractResolver
    {
        internal CamundaRequestJsonResolver()
        {
            NamingStrategy = new CamelCaseNamingStrategy(true, true);
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (member.MemberType == MemberTypes.Field)
            {
                property.Ignored = true;
            }

            if (member.MemberType == MemberTypes.Property && member.GetCustomAttribute(typeof(HttpQueryParameterAttribute)) is HttpQueryParameterAttribute)
            {
                property.Ignored = true;
            }

            return property;
        }
    }
}