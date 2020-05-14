using Renci.SshNet;
using VXDesign.Store.DevTools.Common.Clients.RemoteHost.Entities;
using VXDesign.Store.DevTools.Common.Core.Exceptions;
using VXDesign.Store.DevTools.Common.Core.Operations;

namespace VXDesign.Store.DevTools.Common.Clients.RemoteHost.Extensions
{
    public static class CommandResultExtensions
    {
        internal static CommandResult ToResult(this SshCommand command) => new CommandResult
        {
            Command = command.CommandText,
            Output = command.Result,
            Error = command.Error,
            ExitStatus = command.ExitStatus
        };

        public static CommandResult SendWithErrorHandling(this IRemoteHostClientService client, IOperation operation, string command)
        {
            var result = client.Send(command);
            if (result.HasErrors)
            {
                throw CommonExceptions.FailedToSendCommandToRemoteHost(operation, result.Command, result.ExitStatus, result.Error);
            }

            return result;
        }
    }
}