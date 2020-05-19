using System.Collections.Generic;
using System.Linq;
using VXDesign.Store.DevTools.Common.Clients.Camunda;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Endpoints;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Models.ProcessInstance;
using VXDesign.Store.DevTools.Common.Core.Constants;
using VXDesign.Store.DevTools.Common.Core.Entities.Module;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores;

namespace VXDesign.Store.DevTools.UnifiedPortal.Camunda.Workers.Host
{
    public static class HostModule
    {
        [CamundaWorkerTopic("UnifiedPortal.Host.Module.Search")]
        public class SearchWorker : HostProcessingWorker
        {
            [CamundaWorkerVariable(Name = CamundaWorkerKey.ModuleIdsAndStatuses, Direction = CamundaVariableDirection.Output)]
            public Dictionary<int, ModuleStatus> ModuleIdsAndStatuses { get; set; }

            [CamundaWorkerVariable(Name = CamundaWorkerKey.ModuleProcessInstanceIds, Direction = CamundaVariableDirection.Output)]
            public Dictionary<int, string> ModuleProcessInstanceIds { get; set; } = new Dictionary<int, string>();

            private readonly IModuleStore moduleStore;
            private readonly ISyrinxCamundaClientService camundaClient;

            public SearchWorker(IModuleStore moduleStore, ISyrinxCamundaClientService camundaClient)
            {
                this.moduleStore = moduleStore;
                this.camundaClient = camundaClient;
            }

            public override void Execute(IOperation operation, IOperationLogger logger)
            {
                ModuleIdsAndStatuses = moduleStore.GetHostModuleIds(operation, HostId).Result;
                foreach (var (moduleId, _) in ModuleIdsAndStatuses)
                {
                    var response = new ProcessInstance.GetListRequest
                    {
                        BusinessKey = moduleId.ToString(),
                        ProcessDefinitionKeyIn = new[]
                        {
                            CamundaWorkerKey.ModuleInstallationProcess,
                            CamundaWorkerKey.ModuleUpgradeProcess,
                            CamundaWorkerKey.ModuleRollbackProcess
                        }
                    }.SendRequest(operation, camundaClient, true).Result;

                    foreach (var item in response.Response)
                    {
                        ModuleProcessInstanceIds.Add(int.Parse(item.BusinessKey), item.Id);
                    }
                }
            }
        }

        [CamundaWorkerTopic("UnifiedPortal.Host.Module.Stop")]
        public class StopWorker : HostProcessingWorker
        {
            [CamundaWorkerVariable(Name = CamundaWorkerKey.ModuleIdsAndStatuses)]
            public Dictionary<int, ModuleStatus> ModuleIdsAndStatuses { get; set; }

            [CamundaWorkerVariable(Name = CamundaWorkerKey.ModuleProcessInstanceIds, Direction = CamundaVariableDirection.Input)]
            public Dictionary<int, string> ModuleProcessInstanceIds { get; set; }

            [CamundaWorkerVariable(Name = CamundaWorkerKey.ModuleId, Direction = CamundaVariableDirection.Output)]
            public int? ModuleId { get; set; }

            [CamundaWorkerVariable(Name = CamundaWorkerKey.ComponentsStopRequired, Direction = CamundaVariableDirection.Output)]
            public bool? ComponentsStopRequired { get; set; }

            [CamundaWorkerVariable(Name = CamundaWorkerKey.Index, Direction = CamundaVariableDirection.Output)]
            public int Index { get; set; } // To set 0 automatically

            private readonly IModuleStore moduleStore;
            private readonly ISyrinxCamundaClientService camundaClient;

            public StopWorker(IModuleStore moduleStore, ISyrinxCamundaClientService camundaClient)
            {
                this.moduleStore = moduleStore;
                this.camundaClient = camundaClient;
            }

            public override void Execute(IOperation operation, IOperationLogger logger)
            {
                var moduleIdsAndStatuses = new Dictionary<int, ModuleStatus>();
                foreach (var (id, instanceId) in ModuleProcessInstanceIds)
                {
                    new ProcessInstance.DeleteRequest(instanceId)
                    {
                        FailIfNotExists = false
                    }.SendRequest(operation, camundaClient, true).Wait();

                    moduleStore.ChangeStatus(operation, id, ModuleStatus.UpdatedToUninstall).Wait();
                    moduleIdsAndStatuses.Add(id, ModuleIdsAndStatuses[id]);
                }

                ModuleIdsAndStatuses = moduleIdsAndStatuses;
                var (moduleId, status) = moduleIdsAndStatuses.FirstOrDefault();
                ModuleId = moduleId;
                ComponentsStopRequired = status == ModuleStatus.Run;
            }
        }

        [CamundaWorkerTopic("UnifiedPortal.Host.Module.Calculate")]
        public class CalculateWorker : HostProcessingWorker
        {
            [CamundaWorkerVariable(Name = CamundaWorkerKey.ModuleIdsAndStatuses, Direction = CamundaVariableDirection.Input)]
            public Dictionary<int, ModuleStatus> ModuleIdsAndStatuses { get; set; }

            [CamundaWorkerVariable(Name = CamundaWorkerKey.Index)]
            public int Index { get; set; }

            [CamundaWorkerVariable(Name = CamundaWorkerKey.ModuleId)]
            public int? ModuleId { get; set; }

            [CamundaWorkerVariable(Name = CamundaWorkerKey.ComponentsStopRequired)]
            public bool? ComponentsStopRequired { get; set; }

            public override void Execute(IOperation operation, IOperationLogger logger)
            {
                Index++;

                if (ModuleIdsAndStatuses.Count > Index)
                {
                    ModuleId = ModuleIdsAndStatuses.Keys.ToList()[Index];
                    ComponentsStopRequired = ModuleIdsAndStatuses.Values.ToList()[Index] == ModuleStatus.Run;
                }
                else
                {
                    ModuleId = null;
                    ComponentsStopRequired = null;
                }
            }
        }
    }
}