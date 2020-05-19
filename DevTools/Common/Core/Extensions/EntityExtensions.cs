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

        public static FileExtension? DefineExtension(string fileExtension) => fileExtension switch
        {
            "yml" => FileExtension.YAML,
            "yaml" => FileExtension.YAML,
            "json" => FileExtension.JSON,
            null => FileExtension.Undefined,
            "" => FileExtension.Undefined,
            _ => null
        };
    }
}