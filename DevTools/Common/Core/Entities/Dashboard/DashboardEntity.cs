namespace VXDesign.Store.DevTools.Common.Core.Entities.Dashboard
{
    public class DashboardEntity : IDataEntity
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