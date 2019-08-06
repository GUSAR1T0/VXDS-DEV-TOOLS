using System;
using VXDesign.Store.DevTools.Common.Entities.Camunda;

namespace VXDesign.Store.DevTools.Common.Extensions.Camunda
{
    internal static class CamundaVariableExtensions
    {
        #region Convertors from raw value to instance that is inherited from IVariable

        internal static ICamundaVariable Convert(this bool? value)
        {
            return new BooleanVariable(value);
        }

        internal static ICamundaVariable Convert(this byte[] value)
        {
            return new BytesVariable(value);
        }

        internal static ICamundaVariable Convert(this short? value)
        {
            return new ShortVariable(value);
        }

        internal static ICamundaVariable Convert(this int? value)
        {
            return new IntegerVariable(value);
        }

        internal static ICamundaVariable Convert(this long? value)
        {
            return new LongVariable(value);
        }

        internal static ICamundaVariable Convert(this double? value)
        {
            return new DoubleVariable(value);
        }

        internal static ICamundaVariable Convert(this DateTime? value)
        {
            return new DateTimeVariable(value);
        }

        internal static ICamundaVariable Convert(this DateTimeOffset? value)
        {
            return new DateTimeVariable(value?.UtcDateTime);
        }

        internal static ICamundaVariable Convert(this string value)
        {
            return new StringVariable(value);
        }

        internal static ICamundaVariable Convert(this object value)
        {
            return new JsonVariable(value);
        }

        internal static ICamundaVariable Convert(this CamundaFile value)
        {
            return new FileVariable(value);
        }

        #endregion

        #region Convertors from instance that is inherited from IVariable to raw value

        internal static bool? Convert(this BooleanVariable variable)
        {
            return variable?.Value;
        }

        internal static byte[] Convert(this BytesVariable variable)
        {
            return variable?.Value;
        }

        internal static short? Convert(this ShortVariable variable)
        {
            return variable?.Value;
        }

        internal static int? Convert(this IntegerVariable variable)
        {
            return variable?.Value;
        }

        internal static long? Convert(this LongVariable variable)
        {
            return variable?.Value;
        }

        internal static double? Convert(this DoubleVariable variable)
        {
            return variable?.Value;
        }

        internal static DateTime? Convert(this DateTimeVariable variable)
        {
            return variable?.Value;
        }

        internal static string Convert(this StringVariable variable)
        {
            return variable?.Value;
        }

        internal static object Convert(this JsonVariable variable)
        {
            return variable?.Value;
        }

        internal static CamundaFile Convert(this FileVariable variable)
        {
            return variable?.Value;
        }

        #endregion
    }
}