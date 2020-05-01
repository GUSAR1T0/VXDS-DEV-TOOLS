using System.Collections.Generic;
using Newtonsoft.Json;
using VXDesign.Store.DevTools.Common.Core.Entities.SSP;
using VXDesign.Store.DevTools.Common.Core.Extensions;

namespace VXDesign.Store.DevTools.Common.Core.Entities.Settings
{
    public class HostSettingsItemEntity : IDataEntity, IPagingResponseItemEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public HostOperationSystem OperationSystem { get; set; }
        public IEnumerable<HostCredentialsItemEntity> CredentialsList { get; set; }

        public string Credentials
        {
            get => !CredentialsList.IsNullOrEmpty()
                ? JsonConvert.SerializeObject(CredentialsList, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                })
                : null;
            set => CredentialsList = !string.IsNullOrWhiteSpace(value) ? JsonConvert.DeserializeObject<IEnumerable<HostCredentialsItemEntity>>(value) : new List<HostCredentialsItemEntity>();
        }
    }

    public class HostCredentialsItemEntity
    {
        public HostConnectionType Type { get; set; }
        public int? Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class HostPagingFilter : IPagingFilterEntity
    {
        public IEnumerable<int> Ids { get; set; }
        public IEnumerable<string> Names { get; set; }
        public IEnumerable<string> Domains { get; set; }
        public IEnumerable<HostOperationSystem> OperationSystems { get; set; }
    }

    public class HostPagingRequest : ServerSidePagingRequest<HostPagingFilter>
    {
    }

    public class HostPagingResponse : ServerSidePagingResponse<HostSettingsItemEntity>
    {
    }
}