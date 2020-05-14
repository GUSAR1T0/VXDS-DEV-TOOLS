using System;
using Renci.SshNet;
using VXDesign.Store.DevTools.Common.Clients.RemoteHost.Entities;
using VXDesign.Store.DevTools.Common.Clients.RemoteHost.Extensions;

namespace VXDesign.Store.DevTools.Common.Clients.RemoteHost
{
    public interface ISshClientService : IRemoteHostClientService
    {
    }

    public class SshClientService : ISshClientService
    {
        private readonly SshClient client;

        private SshClientService(SshConnectionByPasswordCredentials credentials)
        {
            client = credentials.Port.HasValue
                ? new SshClient(credentials.Host, credentials.Port.Value, credentials.Username, credentials.Password)
                : new SshClient(credentials.Host, credentials.Username, credentials.Password);
        }

        public static ISshClientService Create(SshConnectionByPasswordCredentials credentials)
        {
            var service = new SshClientService(credentials);
            service.client.Connect();
            return service;
        }

        public CommandResult Send(string command, params string[] arguments) => client.RunCommand($"{command} {string.Join(' ', arguments)}").ToResult();

        public void Dispose()
        {
            client?.Disconnect();
            client?.Dispose();
        }
    }
}