using System;
using System.Collections.Generic;
using System.Linq;
using VXDesign.Store.DevTools.Common.Core.Entities.Settings;
using VXDesign.Store.DevTools.Common.Core.Extensions;

namespace VXDesign.Store.DevTools.Common.Core.Entities.Module
{
    #region Common

    public class ConfigurationCommand
    {
        public string Run { get; set; }
    }

    public interface IConfigurationFlow
    {
        bool IsValid();
    }

    #endregion

    #region Configurations

    public abstract class MigrationConfiguration : IConfigurationFlow
    {
        public IEnumerable<ConfigurationCommand> Upgrade { get; set; }
        public IEnumerable<ConfigurationCommand> Rollback { get; set; }
        public IEnumerable<ConfigurationCommand> Downgrade { get; set; }

        public bool IsValid() => Upgrade.AreValid() &&
                                 Rollback.AreValid() &&
                                 Downgrade.AreValid();
    }

    public abstract class ApplicationFlow : IConfigurationFlow
    {
        public IEnumerable<ConfigurationCommand> Launch { get; set; }
        public IEnumerable<ConfigurationCommand> Stop { get; set; }

        public bool IsValid() => Launch.AreValid(false) &&
                                 Stop.AreValid(false);
    }

    public class DatabaseConfiguration : MigrationConfiguration
    {
    }

    public class CamundaWorkflowsConfiguration : MigrationConfiguration
    {
    }

    public class CamundaWorkersConfiguration : ApplicationFlow
    {
    }

    public class CamundaConfiguration : IConfigurationFlow
    {
        public CamundaWorkflowsConfiguration Workflows { get; set; }
        public CamundaWorkersConfiguration Workers { get; set; }

        public bool IsValid() => (Workflows == null || Workflows.IsValid()) &&
                                 (Workers == null || Workers.IsValid());
    }

    public class ApplicationConfiguration : ApplicationFlow
    {
    }

    #endregion

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

        public IEnumerable<ConfigurationCommand> BeforeStep { get; set; }
        public DatabaseConfiguration Database { get; set; }
        public CamundaConfiguration Camunda { get; set; }
        public ApplicationConfiguration Application { get; set; }
        public IEnumerable<ConfigurationCommand> AfterStep { get; set; }

        public bool IsValid() => OperatingSystem != null &&
                                 BeforeStep.AreValid() && (
                                     Database == null ||
                                     Database.IsValid()
                                 ) && (
                                     Camunda == null ||
                                     Camunda.IsValid()
                                 ) &&
                                 Application != null &&
                                 Application.IsValid() &&
                                 AfterStep.AreValid();
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