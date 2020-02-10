namespace VXDesign.Store.DevTools.UnifiedPortal.Models.Dashboard
{
    public class DashboardModel
    {
        #region Users

        public int UsersCount { get; set; }
        public int RolesCount { get; set; }

        #endregion

        #region System

        public long OperationsCount { get; set; }
        public long LogsCount { get; set; }

        #endregion
    }
}