using System;
using System.Collections.Generic;
using VXDesign.Store.DevTools.Common.Clients.RemoteHost.Entities;
using VXDesign.Store.DevTools.Common.Core.Entities.Settings;

namespace VXDesign.Store.DevTools.Common.Clients.RemoteHost.Extensions
{
    public static class RemoteHostClientExtensions
    {
        public static CommandResult CheckConnection(this HostCredentialsItemEntity credentials, HostOperatingSystem operatingSystem, string host, out IRemoteHostClientService client)
        {
            client = null;

            string command;
            var arguments = new List<string>();
            switch (operatingSystem)
            {
                case HostOperatingSystem.MacOS:
                case HostOperatingSystem.Linux:
                    command = "uname";
                    arguments.Add("-a");
                    break;
                case HostOperatingSystem.WindowsOS:
                    command = "ver";
                    break;
                default:
                    command = "";
                    break;
            }

            if (credentials.Type == HostConnectionType.SSH)
            {
                try
                {
                    client = SshClientService.Create(new SshConnectionByPasswordCredentials
                    {
                        Host = host,
                        Port = credentials.Port,
                        Username = credentials.Username,
                        Password = credentials.Password
                    });
                    return client.Send(command, arguments.ToArray());
                }
                catch (Exception e)
                {
                    return new CommandResult
                    {
                        Command = $"{command} {string.Join(' ', arguments)}",
                        Output = null,
                        Error = e.Message,
                        ExitStatus = -1
                    };
                }
            }

            return null;
        }
    }
}