using Renci.SshNet;
using VXDesign.Store.DevTools.Common.Clients.RemoteHost.Entities;

namespace VXDesign.Store.DevTools.Common.Clients.RemoteHost.Extensions
{
    internal static class CommandResultExtensions
    {
        internal static CommandResult ToResult(this SshCommand command) => new CommandResult
        {
            Command = command.CommandText,
            Output = command.Result,
            Error = command.Error,
            ExitStatus = command.ExitStatus
        };
    }
}