using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VXDesign.Store.DevTools.Common.Extensions.Camunda;
using VXDesign.Store.DevTools.Common.Utils.Camunda;

namespace VXDesign.Store.DevTools.Common.Containers.Camunda.Base
{
    public interface ICamundaVariable
    {
        T To<T>();

        string ToString();
    }

    public abstract class CamundaVariable<TValue> : ICamundaVariable
    {
        public CamundaVariable(TValue value)
        {
            Value = value;
        }

        public abstract string Type { get; }

        [JsonIgnore]
        public TValue Value { get; }

        [JsonProperty(nameof(Value))]
        public abstract string StringValue { get; }

        public virtual Dictionary<string, object> ValueInfo { get; } = null;

        public abstract T To<T>();

        public override string ToString() => StringValue;
    }

    #region Variable Types Implementation

    public class BooleanVariable : CamundaVariable<bool?>
    {
        public const string TypeName = "boolean";

        internal BooleanVariable(bool? value) : base(value)
        {
        }

        public override string Type => TypeName;
        public override string StringValue => Value?.ToString(CultureInfo.InvariantCulture);

        public override T To<T>() => CamundaVariablesUtils.ToValueTypedObject<T, bool>(this.Convert);
    }

    public class BytesVariable : CamundaVariable<byte[]>
    {
        public const string TypeName = "bytes";

        internal BytesVariable(byte[] value) : base(value)
        {
        }

        public override string Type => TypeName;
        public override string StringValue => Convert.ToBase64String(Value ?? new byte[] { });

        public override T To<T>() => CamundaVariablesUtils.ToReferencedObject<T, byte[]>(this.Convert);
    }

    public class ShortVariable : CamundaVariable<short?>
    {
        public const string TypeName = "short";

        internal ShortVariable(short? value) : base(value)
        {
        }

        public override string Type => TypeName;
        public override string StringValue => Value?.ToString(CultureInfo.InvariantCulture);

        public override T To<T>() => CamundaVariablesUtils.ToValueTypedObject<T, short>(this.Convert);
    }

    public class IntegerVariable : CamundaVariable<int?>
    {
        public const string TypeName = "integer";

        internal IntegerVariable(int? value) : base(value)
        {
        }

        public override string Type => TypeName;
        public override string StringValue => Value?.ToString(CultureInfo.InvariantCulture);

        public override T To<T>() => CamundaVariablesUtils.ToValueTypedObject<T, int>(this.Convert);
    }

    public class LongVariable : CamundaVariable<long?>
    {
        public const string TypeName = "long";

        internal LongVariable(long? value) : base(value)
        {
        }

        public override string Type => TypeName;
        public override string StringValue => Value?.ToString(CultureInfo.InvariantCulture);

        public override T To<T>() => CamundaVariablesUtils.ToValueTypedObject<T, long>(this.Convert);
    }

    public class DoubleVariable : CamundaVariable<double?>
    {
        public const string TypeName = "double";

        internal DoubleVariable(double? value) : base(value)
        {
        }

        public override string Type => TypeName;
        public override string StringValue => Value?.ToString(CultureInfo.InvariantCulture);

        public override T To<T>() => CamundaVariablesUtils.ToValueTypedObject<T, double>(this.Convert);
    }

    public class DecimalVariable : JsonVariable
    {
        public const string ExtendTypeName = "decimal";

        public DecimalVariable(decimal? value) : base(value)
        {
        }

        public override string StringValue => JsonConvert.SerializeObject(new Dictionary<string, object>
        {
            { "value", Convert.ToString(Value) },
            { "extendType", ExtendTypeName }
        });

        public override T To<T>() => CamundaVariablesUtils.ToValueTypedObject<T, decimal>(this.Convert);
    }

    public class DateTimeVariable : CamundaVariable<DateTime?>
    {
        public const string TypeName = "date";

        internal DateTimeVariable(DateTime? value) : base(value)
        {
        }

        public override string Type => TypeName;
        public override string StringValue => Value?.ToString("yyyy-MM-ddTHH:mm:ss.fff") + Value?.ToString("zzz").Replace(":", "");

        public override T To<T>() => CamundaVariablesUtils.ToValueTypedObject<T, DateTime>(this.Convert);
    }

    public class StringVariable : CamundaVariable<string>
    {
        public const string TypeName = "string";

        internal StringVariable(string value) : base(value)
        {
        }

        public override string Type => TypeName;
        public override string StringValue => Value;

        public override T To<T>() => CamundaVariablesUtils.ToReferencedObject<T, string>(this.Convert);
    }

    public class JsonVariable : CamundaVariable<object>
    {
        public const string TypeName = "json";

        internal JsonVariable(object value) : base(value)
        {
        }

        public override string Type => TypeName;
        public override string StringValue => JsonConvert.SerializeObject(Value);

        public override T To<T>()
        {
            var variable = this.Convert();
            switch (variable)
            {
                case JObject obj:
                    return obj.ToObject<T>();
                case JArray arr:
                    return arr.ToObject<T>();
                default:
                    return (T) variable;
            }
        }
    }

    public class FileVariable : CamundaVariable<CamundaFile>
    {
        public const string TypeName = "file";

        internal FileVariable(CamundaFile value) : base(value)
        {
        }

        public override string Type => TypeName;
        public override string StringValue => Convert.ToBase64String(Value.Data ?? new byte[] { });

        public override Dictionary<string, object> ValueInfo => new Dictionary<string, object>
        {
            { "filename", Value.FileName },
            { "mimeType", Value.MimeType },
            { "encoding", Value.Encoding }
        };

        public override T To<T>() => CamundaVariablesUtils.ToReferencedObject<T, CamundaFile>(this.Convert);
    }

    #endregion

    #region Variable Collector

    public interface IReadOnlyCamundaVariables : IReadOnlyDictionary<string, ICamundaVariable>
    {
    }

    public class CamundaVariables : Dictionary<string, ICamundaVariable>, IReadOnlyCamundaVariables
    {
        public void Add(string key, bool? value)
        {
            this[key] = value.Convert();
        }

        public void Add(string key, byte[] value)
        {
            this[key] = value.Convert();
        }

        public void Add(string key, short? value)
        {
            this[key] = value.Convert();
        }

        public void Add(string key, int? value)
        {
            this[key] = value.Convert();
        }

        public void Add(string key, long? value)
        {
            this[key] = value.Convert();
        }

        public void Add(string key, double? value)
        {
            this[key] = value.Convert();
        }

        public void Add(string key, decimal? value)
        {
            this[key] = value.Convert();
        }

        public void Add(string key, DateTime? value)
        {
            this[key] = value.Convert();
        }

        public void Add(string key, DateTimeOffset? value)
        {
            this[key] = value.Convert();
        }

        public void Add(string key, string value)
        {
            this[key] = value.Convert();
        }

        public void Add(string key, object value)
        {
            this[key] = value.Convert();
        }

        public void Add(string key, CamundaFile value)
        {
            this[key] = value.Convert();
        }
    }

    #endregion
}