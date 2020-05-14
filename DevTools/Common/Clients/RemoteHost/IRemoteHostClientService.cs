using System;
using VXDesign.Store.DevTools.Common.Clients.RemoteHost.Entities;

namespace VXDesign.Store.DevTools.Common.Clients.RemoteHost
{
    public interface IRemoteHostClientService : IDisposable
    {
        CommandResult Send(string command, params string[] arguments);
    }
}