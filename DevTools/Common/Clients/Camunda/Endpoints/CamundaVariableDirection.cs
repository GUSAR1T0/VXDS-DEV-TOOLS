using System;

namespace VXDesign.Store.DevTools.Common.Clients.Camunda.Endpoints
{
    [Flags]
    public enum CamundaVariableDirection
    {
        Input = 1,
        Output = 2
    }
}