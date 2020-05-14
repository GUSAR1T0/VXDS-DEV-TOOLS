using System.Collections.Generic;
using System.Linq;
using VXDesign.Store.DevTools.Common.Core.Entities.Settings;

namespace VXDesign.Store.DevTools.Common.Core.Entities.Module
{
    public class ModuleShortEntity : IDataEntity
    {
        public int Id { get; set; }
        public string Alias { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }

        public ModuleStatus Status { get; set; }
    }

    public class ModuleEntity : ModuleShortEntity
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Color { get; set; }

        public int HostId { get; set; }
        public string HostName { get; set; }
        public string HostDomain { get; set; }
        public HostOperatingSystem HostOperatingSystem { get; set; }

        public List<ModuleConfigurationEntity> Configurations { get; set; }

        public ModuleConfigurationEntity PreviousConfiguration
        {
            get
            {
                for (var i = 1; i < Configurations.Count; i++)
                {
                    if (Configurations[i].Version == Version)
                    {
                        return Configurations[i - 1];
                    }
                }

                return null;
            }
        }

        public ModuleConfigurationEntity CurrentConfiguration => Configurations.FirstOrDefault(configuration => configuration.Version == Version);

        public ModuleConfigurationEntity NextConfiguration
        {
            get
            {
                for (var i = 0; i < Configurations.Count - 1; i++)
                {
                    if (Configurations[i].Version == Version)
                    {
                        return Configurations[i + 1];
                    }
                }

                return null;
            }
        }
    }
}