using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Extensions;

namespace VXDesign.Store.DevTools.Common.Clients.Camunda.Utils
{
    internal class CamundaVariableJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
        }

        public override object ReadJson(JsonReader reader, Type type, object existingValue, JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);

            switch (((string) obj["type"]).ToLowerInvariant())
            {
                case BooleanVariable.TypeName:
                    return ((bool?) obj["value"]).Convert();
                case BytesVariable.TypeName:
                    return ((byte[]) obj["value"]).Convert();
                case ShortVariable.TypeName:
                    return ((short?) obj["value"]).Convert();
                case IntegerVariable.TypeName:
                    return ((int?) obj["value"]).Convert();
                case LongVariable.TypeName:
                    return ((long?) obj["value"]).Convert();
                case DoubleVariable.TypeName:
                    return ((double?) obj["value"]).Convert();
                case DateTimeVariable.TypeName:
                    return ((DateTime?) obj["value"]).Convert();
                case StringVariable.TypeName:
                    return ((string) obj["value"]).Convert();
                case JsonVariable.TypeName:
                    var json = JsonConvert.DeserializeObject((string) obj["value"]);
                    if (json is JObject objectContent && objectContent.ContainsKey("extendType") && (string) objectContent["extendType"] == DecimalVariable.ExtendTypeName)
                    {
                        // decimal
                        var decimalStringValue = (string) objectContent["value"];
                        var decimalConvertedValue = Convert.ToDecimal(decimalStringValue);
                        return ((decimal?) decimalConvertedValue).Convert();
                    }

                    // object
                    return json.Convert();
                case FileVariable.TypeName:
                    var data = obj["value"].Type != JTokenType.Null ? (byte[]) obj["value"] : null;
                    var fileName = (string) obj["valueInfo"]["filename"];
                    var mimeType = (string) obj["valueInfo"]["mimeType"];
                    var encoding = (string) obj["valueInfo"]["encoding"];
                    return new CamundaFile(data, fileName, mimeType, encoding).Convert();
                default:
                    return null;
            }
        }

        public override bool CanConvert(Type type) => type == typeof(ICamundaVariable);
    }
}