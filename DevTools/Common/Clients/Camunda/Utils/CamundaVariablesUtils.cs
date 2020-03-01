using System;

namespace VXDesign.Store.DevTools.Common.Clients.Camunda.Utils
{
    internal static class CamundaVariablesUtils
    {
        internal static TExpected ToValueTypedObject<TExpected, TReal>(Func<TReal?> variable) where TReal : struct
        {
            return (TExpected) (typeof(TExpected) == typeof(TReal) || typeof(TExpected) == typeof(TReal?) ? (object) variable() : default(TExpected));
        }

        internal static object ToValueTypedObject<TReal>(Type expected, Func<TReal?> variable) where TReal : struct
        {
            return expected == typeof(TReal) || expected == typeof(TReal?) ? (object) variable() : null;
        }

        internal static TExpected ToReferencedObject<TExpected, TReal>(Func<TReal> variable) where TReal : class
        {
            return (TExpected) (typeof(TExpected) == typeof(TReal) ? (object) variable() : default(TExpected));
        }

        internal static object ToReferencedObject<TReal>(Type expected, Func<TReal> variable) where TReal : class
        {
            return expected == typeof(TReal) ? variable() : null;
        }
    }
}