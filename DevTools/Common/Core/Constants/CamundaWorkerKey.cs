namespace VXDesign.Store.DevTools.Common.Core.Constants
{
    public static class CamundaWorkerKey
    {
        #region Definition Keys

        public const string ModuleLaunchProcess = "module-launch-process";
        public const string ModuleStopProcess = "module-stop-process";
        public const string ModuleInstallationProcess = "module-installation-process";
        public const string ModuleUpgradeProcess = "module-upgrade-process";
        public const string ModuleRollbackProcess = "module-rollback-process";
        public const string ModuleUninstallationProcess = "module-uninstallation-process";
        public const string HostDeletionProcess = "host-deletion-process";
        public const string NoteNotificationProcess = "note-notification-process";

        #endregion

        #region BPMN Errors

        public const string ModuleProcessingError = "module-processing-error";

        #endregion

        #region Variables

        public const string ModuleId = "moduleId";
        public const string ComponentsStopRequired = "componentsStopRequired";
        public const string ModuleStatus = "moduleStatus";
        public const string ModuleIdsAndStatuses = "moduleIdsAndStatuses";
        public const string HostId = "hostId";
        public const string ModuleProcessInstanceIds = "moduleProcessInstanceIds";
        public const string Index = "index";
        public const string NoteId = "noteId";
        public const string UserIds = "userIds";
        public const string ErrorMessage = "errorMessage";
        public const string Action = "action";

        #endregion
    }
}