using System;

namespace VXDesign.Store.DevTools.Common.Utils.Camunda
{
    public static class CamundaVariablesUtils
    {
        internal static TExpected ToValueTypedObject<TExpected, TReal>(Func<TReal?> variable) where TReal : struct
        {
            return (TExpected) (typeof(TExpected) == typeof(TReal) || typeof(TExpected) == typeof(TReal?) ? (object) variable() : default(TExpected));
        }

        internal static TExpected ToReferencedObject<TExpected, TReal>(Func<TReal> variable) where TReal : class
        {
            return (TExpected) (typeof(TExpected) == typeof(TReal) ? (object) variable() : default(TExpected));
        }
    }
}