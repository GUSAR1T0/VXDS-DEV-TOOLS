using VXDesign.Store.DevTools.Core.Attributes;

namespace VXDesign.Store.DevTools.Core.Enums.Camunda
{
    public enum CamundaCategory
    {
        [CamundaCategory("Authorization", "authorization")]
        Authorization = 1,

        [CamundaCategory("Batch", "batch")]
        Batch = 2,

        [CamundaCategory("Case Definition", "case-definition")]
        CaseDefinition = 3,

        [CamundaCategory("Case Execution", "case-execution")]
        CaseExecution = 4,

        [CamundaCategory("Case Instance", "case-instance")]
        CaseInstance = 5,

        [CamundaCategory("Condition", "condition")]
        Condition = 6,

        [CamundaCategory("Decision Definition", "decision-definition")]
        DecisionDefinition = 7,

        [CamundaCategory("Decision Requirements Definition", "decision-requirements-definition")]
        DecisionRequirementsDefinition = 8,

        [CamundaCategory("Deployment", "deployment")]
        Deployment = 9,

        [CamundaCategory("Engine", "engine")]
        Engine = 10,

        [CamundaCategory("Execution", "execution")]
        Execution = 11,

        [CamundaCategory("External Task", "external-task")]
        ExternalTask = 12,

        [CamundaCategory("Filter", "filter")]
        Filter = 13,

        [CamundaCategory("Group", "group")]
        Group = 14,

        [CamundaCategory("History", "history")]
        History = 15,

        [CamundaCategory("Identity", "identity")]
        Identity = 16,

        [CamundaCategory("Incident", "incident")]
        Incident = 17,

        [CamundaCategory("Job", "job")]
        Job = 18,

        [CamundaCategory("Job Definition", "job-definition")]
        JobDefinition = 19,

        [CamundaCategory("Message", "message")]
        Message = 20,

        [CamundaCategory("Metrics", "metrics")]
        Metrics = 21,

        [CamundaCategory("Migration", "migration")]
        Migration = 22,

        [CamundaCategory("Modification", "modification")]
        Modification = 23,

        [CamundaCategory("Process Definition", "process-definition")]
        ProcessDefinition = 24,

        [CamundaCategory("Process Instance", "process-instance")]
        ProcessInstance = 25,

        [CamundaCategory("Signal", "signal")]
        Signal = 26,

        [CamundaCategory("Schema Log", "schema")]
        SchemaLog = 27,

        [CamundaCategory("Task", "task")]
        Task = 28,

        [CamundaCategory("Tenant", "tenant")]
        Tenant = 29,

        [CamundaCategory("User", "user")]
        User = 30,

        [CamundaCategory("Variable Instance", "variable-instance")]
        VariableInstance = 31
    }
}