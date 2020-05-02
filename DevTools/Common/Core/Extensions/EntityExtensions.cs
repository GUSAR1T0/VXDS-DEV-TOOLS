using System;
using System.Collections.Generic;
using System.Linq;
using VXDesign.Store.DevTools.Common.Core.Entities.File;
using VXDesign.Store.DevTools.Common.Core.Entities.Module;

namespace VXDesign.Store.DevTools.Common.Core.Extensions
{
    public static class EntityExtensions
    {
        public static bool AreValid(this IEnumerable<ConfigurationCommand> commands, bool canBeEmpty = true)
        {
            var commandsList = commands?.ToList();
            var listChecker = new Func<bool>(() => commandsList?.All(command => !string.IsNullOrWhiteSpace(command.Run)) == true);
            return canBeEmpty ? commandsList.IsNullOrEmpty() || listChecker() : !commandsList.IsNullOrEmpty() && listChecker();
        }

        public static FileExtension? DefineExtension(string fileExtension)
        {
            switch (fileExtension?.ToLower())
            {
                case "yml":
                case "yaml":
                    return FileExtension.YAML;
                case "json":
                    return FileExtension.JSON;
                case null:
                case "":
                    return FileExtension.Undefined;
                default:
                    return null;
            }
        }
    }
}