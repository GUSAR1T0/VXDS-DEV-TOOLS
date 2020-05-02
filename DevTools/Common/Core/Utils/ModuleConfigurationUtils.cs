using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using VXDesign.Store.DevTools.Common.Core.Entities.File;
using VXDesign.Store.DevTools.Common.Core.Entities.Module;
using VXDesign.Store.DevTools.Common.Core.Exceptions;
using VXDesign.Store.DevTools.Common.Core.Operations;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace VXDesign.Store.DevTools.Common.Core.Utils
{
    public static class ModuleConfigurationUtils
    {
        public static ModuleConfigurationFile Parse(IOperation operation, UploadedFile file) => file.Extension switch
        {
            FileExtension.YAML => ParseYaml(file.Content),
            FileExtension.JSON => ParseJson(file.Content),
            FileExtension.Undefined => throw CommonExceptions.UploadedFileExtensionIsUndefined(operation, FileExtension.YAML, FileExtension.JSON),
            _ => throw CommonExceptions.UnexpectedExtensionOfUploadedFile(operation, file.SourceExtension, FileExtension.YAML, FileExtension.JSON)
        };

        private static ModuleConfigurationFile ParseYaml(string content) => new DeserializerBuilder()
            .IgnoreUnmatchedProperties()
            .WithNamingConvention(HyphenatedNamingConvention.Instance)
            .Build()
            .Deserialize<ModuleConfigurationFile>(content);

        private static ModuleConfigurationFile ParseJson(string content) => JsonConvert.DeserializeObject<ModuleConfigurationFile>(content, new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new KebabCaseNamingStrategy(true, true)
            }
        });
    }
}