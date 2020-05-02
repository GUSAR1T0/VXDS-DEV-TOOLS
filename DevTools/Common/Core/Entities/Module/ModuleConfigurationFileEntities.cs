using System;
using System.Collections.Generic;
using System.Linq;
using VXDesign.Store.DevTools.Common.Core.Entities.Settings;
using VXDesign.Store.DevTools.Common.Core.Extensions;

namespace VXDesign.Store.DevTools.Common.Core.Entities.Module
{
    public class ConfigurationCommand
    {
        public string Run { get; set; }
    }

    public class DatabaseConfigurationFlow
    {
        public IEnumerable<ConfigurationCommand> Install { get; set; }
        public IEnumerable<ConfigurationCommand> Upgrade { get; set; }
        public IEnumerable<ConfigurationCommand> Downgrade { get; set; }
        public IEnumerable<ConfigurationCommand> Uninstall { get; set; }

        public bool IsValid() => Install.AreValid() &&
                                 Upgrade.AreValid() &&
                                 Downgrade.AreValid() &&
                                 Uninstall.AreValid();
    }

    public class CamundaConfigurationFlow
    {
        public IEnumerable<ConfigurationCommand> Install { get; set; }
        public IEnumerable<ConfigurationCommand> Upgrade { get; set; }
        public IEnumerable<ConfigurationCommand> Downgrade { get; set; }
        public IEnumerable<ConfigurationCommand> Uninstall { get; set; }

        public bool IsValid() => Install.AreValid() &&
                                 Upgrade.AreValid() &&
                                 Downgrade.AreValid() &&
                                 Uninstall.AreValid();
    }

    public class ApplicationConfigurationFlow
    {
        public IEnumerable<ConfigurationCommand> Launch { get; set; }
        public IEnumerable<ConfigurationCommand> Stop { get; set; }

        public bool IsValid() => Launch.AreValid(false) &&
                                 Stop.AreValid(false);
    }

    public class OperatingSystemInstructions
    {
        public string OsName { get; set; }

        public HostOperatingSystem? OperatingSystem
        {
            get
            {
                var osName = OsName.Replace(" ", "");
                foreach (var value in EnumExtensions.GetValues<HostOperatingSystem>())
                {
                    var operatingSystem = value.GetDescription().Replace(" ", "");
                    if (operatingSystem.Equals(osName, StringComparison.InvariantCultureIgnoreCase)) return value;
                }

                return null;
            }
        }

        public IEnumerable<ConfigurationCommand> BeforeAll { get; set; }
        public DatabaseConfigurationFlow Database { get; set; }
        public CamundaConfigurationFlow Camunda { get; set; }
        public ApplicationConfigurationFlow Application { get; set; }
        public IEnumerable<ConfigurationCommand> AfterAll { get; set; }

        public bool IsValid() => OperatingSystem != null &&
                                 BeforeAll.AreValid() && (
                                     Database == null ||
                                     Database.IsValid()
                                 ) && (
                                     Camunda == null ||
                                     Camunda.IsValid()
                                 ) &&
                                 Application != null &&
                                 Application.IsValid() &&
                                 AfterAll.AreValid();
    }

    public class ModuleConfigurationFile
    {
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Version { get; set; }
        public string Author { get; set; }
        public string Email { get; set; }
        public IEnumerable<OperatingSystemInstructions> Instructions { get; set; }

        public bool IsValid() => !string.IsNullOrWhiteSpace(Name) &&
                                 !string.IsNullOrWhiteSpace(Alias) &&
                                 !string.IsNullOrWhiteSpace(Version) &&
                                 !string.IsNullOrWhiteSpace(Author) &&
                                 !string.IsNullOrWhiteSpace(Email) &&
                                 !Instructions.IsNullOrEmpty() &&
                                 Instructions.All(item => item.IsValid());
    }
}