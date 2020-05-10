namespace VXDesign.Store.DevTools.Common.Migrations.Common
{
    public interface IMigrationService
    {
        void Upgrade();
        void DowngradeToPrevious();
        void Downgrade();
    }
}