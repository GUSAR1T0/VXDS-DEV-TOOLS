using System;
using VXDesign.Store.DevTools.Common.Entities.Camunda.Base;

namespace VXDesign.Store.DevTools.Common.Extensions.Camunda
{
    internal static class CamundaVariableExtensions
    {
        #region Convertors from raw value to instance that is inherited from IVariable

        internal static ICamundaVariable Convert(this bool? value) => new BooleanVariable(value);

        internal static ICamundaVariable Convert(this byte[] value) => new BytesVariable(value);

        internal static ICamundaVariable Convert(this short? value) => new ShortVariable(value);

        internal static ICamundaVariable Convert(this int? value) => new IntegerVariable(value);

        internal static ICamundaVariable Convert(this long? value) => new LongVariable(value);

        internal static ICamundaVariable Convert(this double? value) => new DoubleVariable(value);

        internal static ICamundaVariable Convert(this decimal? value) => new DecimalVariable(value);

        internal static ICamundaVariable Convert(this DateTime? value) => new DateTimeVariable(value);

        internal static ICamundaVariable Convert(this DateTimeOffset? value) => new DateTimeVariable(value?.UtcDateTime);

        internal static ICamundaVariable Convert(this string value) => new StringVariable(value);

        internal static ICamundaVariable Convert(this object value) => new JsonVariable(value);

        internal static ICamundaVariable Convert(this CamundaFile value) => new FileVariable(value);

        #endregion

        #region Convertors from instance that is inherited from IVariable to raw value

        internal static bool? Convert(this BooleanVariable variable) => variable?.Value;

        internal static byte[] Convert(this BytesVariable variable) => variable?.Value;

        internal static short? Convert(this ShortVariable variable) => variable?.Value;

        internal static int? Convert(this IntegerVariable variable) => variable?.Value;

        internal static long? Convert(this LongVariable variable) => variable?.Value;

        internal static double? Convert(this DoubleVariable variable) => variable?.Value;

        internal static decimal? Convert(this DecimalVariable variable) => (decimal?) variable?.Value;

        internal static DateTime? Convert(this DateTimeVariable variable) => variable?.Value;

        internal static string Convert(this StringVariable variable) => variable?.Value;

        internal static object Convert(this JsonVariable variable) => variable?.Value;

        internal static CamundaFile Convert(this FileVariable variable) => variable?.Value;

        #endregion
    }
}