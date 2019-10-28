namespace VXDesign.Store.DevTools.Common.Entities.Storage
{
    public class DashboardEntity : IDataEntity
    {
        public int UsersCount { get; set; }
        public int RolesCount { get; set; }
        public long LogsCount { get; set; }
    }
}